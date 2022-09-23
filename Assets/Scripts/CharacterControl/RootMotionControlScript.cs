using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class RootMotionControlScript : MonoBehaviour
{
    private Animator anim;	
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    public float initialMatchTargetsAnimTime = 0.25f;
    public float exitMatchTargetsAnimTime = 0.75f;
	public Camera camera;

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
    public float rootTurnSpeed = 1f;

	private int groundContactCount = 0;
    
	private Plane plane;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

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
        
	    // Derived stack overflow post: https://stackoverflow.com/questions/61864195/how-do-i-rotate-my-player-based-on-my-current-world-mouse-position-3d-isometric
	    //Create a ray from the Mouse position into the scene
	    var ray = camera.ScreenPointToRay(Input.mousePosition);

	    // Use this ray to Raycast against the mathematical floor plane
	    // "enter" will be a float holding the distance from the camera 
	    // to the point where the ray hit the plane
	    if (plane.Raycast(ray, out var enter))
	    {
		    //Get the 3D world point where the ray hit the plane
		    var hitPoint = ray.GetPoint(enter);

		    // project the player position onto the plane so you get the position
		    // only in XZ and can directly compare it to the mouse ray hit
		    // without any difference in the Y axis
		    var playerPositionOnPlane = plane.ClosestPointOnPlane(transform.position);

		    // rotate the player so it face the same direction as the one from the playerPositionOnPlane -> hitPoint 
		    transform.rotation = Quaternion.LookRotation(hitPoint-playerPositionOnPlane);
	    }
        
    }
     

    void FixedUpdate()
    {


        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground.
        //This is good for allowing player to jump and not be frustrated that the jump button doesn't
        //work
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);



        if(_inputActionFired)
        {
            _inputActionFired = false; // clear the input event that came from Update()

            Debug.Log("Action pressed");
        }


        // TODO HANDLE BUTTON MATCH TARGET HERE
        // get info about current animation
	    var animState = anim.GetCurrentAnimatorStateInfo(0);


	    anim.speed = animationSpeed;
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isFalling", !isGrounded);

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

        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

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
