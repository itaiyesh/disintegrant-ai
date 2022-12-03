using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController)), RequireComponent(typeof(WeaponController))]
public class CharacterController : MonoBehaviour
{
    public GameObject Ragdoll;
    private WeaponController weaponController;
    public Transform aimTarget;
    public Rig aimLayer;
    public LayerMask aimLayerMask = new LayerMask();

    private Animator anim;
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    public float initialMatchTargetsAnimTime = 0.25f;
    public float exitMatchTargetsAnimTime = 0.75f;
    private Camera playerCamera;

    // classic input system only polls in Update()
    // so must treat input events like discrete button presses as
    // "triggered" until consumed by FixedUpdate()...
    bool _inputActionFired = false;

    // ...however constant input measures like axes can just have most recent value
    // cached.
    float _inputForward = 0f;
    float _inputTurn = 0f;


    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1.175f;

    private int groundContactCount = 0;

    private Plane plane;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    public Transform playerCameraRoot;

    private float distanceToGround;
    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (playerCamera == null)
            Debug.Log("Camera could not be found");

        weaponController = GetComponent<WeaponController>();

        distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;

    }

    // Use this for initialization
    void Start()
    {
        //example of how to get access to certain limbs
        leftFoot = this.transform.Find("Root/Hips/UpperLeg_L/LowerLeg_L/Ankle_L");
        rightFoot = this.transform.Find("Root/Hips/UpperLeg_R/LowerLeg_R/Ankle_R");
        plane = new Plane(Vector3.up, Vector3.zero);

        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");

    }

    private void Update()
    {

        if (cinput.enabled)
        {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;

            // Note that we don't overwrite a true value already stored
            // Is only cleared to false in FixedUpdate()
            // This makes certain that the action is handled!
            _inputActionFired = _inputActionFired || cinput.Action;

        }

        // var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        var ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        // Aim target
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimLayerMask))
        {
            aimTarget.position = raycastHit.point;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            weaponController.Attack(aimTarget.position, Input.GetMouseButtonDown(0) ? WeaponFireType.SINGLE : WeaponFireType.RAPID);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyUp(KeyCode.C))
        {
            weaponController.NextWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            weaponController.PreviousWeapon();
        }

        PlayerRotation();

        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground.
        //This is good for allowing player to jump and not be frustrated that the jump button doesn't
        //work

        //TODO: This ray cast doesnt really work.
        bool isGrounded = IsGrounded || CharacterUtils.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);
        // Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        if (_inputActionFired)
        {
            _inputActionFired = false; // clear the input event that came from Update()
        }


        anim.speed = animationSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.speed *= 2.0f;
            _inputForward *= 1.5f;
            _inputTurn *= 1.5f;
        }

        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isFalling", !isGrounded);

        if (Input.GetKeyDown(KeyCode.Y)) { Die(); }

    }
    private const float _threshold = 0.01f;
    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
            return false;
#endif
        }
    }
    // cinemachine
    private float _cinemachineTargetYaw = 0;
    private float _cinemachineTargetPitch = 0;
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    private void PlayerRotation()
    {

        // if there is an input and camera position is not fixed
        if (cinput.Look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _cinemachineTargetYaw += cinput.Look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += cinput.Look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        playerCameraRoot.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
        _cinemachineTargetYaw, 0.0f);
        //For actual player rotation, we reset pitch (So player wont rotate downward/upward)
        transform.rotation = Quaternion.Euler(CameraAngleOverride,
        _cinemachineTargetYaw, 0.0f);

    }

    void Die()
    {
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(Ragdoll, transform.position, transform.rotation);
        Destroy(ragdoll, 30);
    }

    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;

            // Generate an event that might play a sound, generate a particle effect, etc.
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }

    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded || CharacterUtils.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        if (isGrounded)
        {
            //use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower

        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, rootTurnSpeed);



        // old way
        //this.transform.position = newRootPosition;
        //this.transform.rotation = newRootRotation;

        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }

}
