using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanningBot : MonoBehaviour
{
    public enum State
    {
        FORWARD,
        ROTATE
    }
    private float lastStateTime;
    public State state;

    public float speed = 10f;
    public float turnSpeed = 0.5f;

    public float forwardDuration = 10f;
    public float turnDuration = 2f;
    private Rigidbody rbody;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        SetState(State.FORWARD);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.FORWARD:
                animator.SetBool("forward", true);
                if (Time.fixedTime - lastStateTime > forwardDuration)
                {
                    //If too long has passed, switch to rotate
                    SetState(State.ROTATE);
                    break;
                }
                rbody.velocity = transform.forward * speed;
                rbody.angularVelocity = Vector3.zero;
                break;

            case State.ROTATE:
                animator.SetBool("forward", false);
                if (Time.fixedTime - lastStateTime > turnDuration)
                {
                    SetState(State.FORWARD);
                    break;
                }
                rbody.velocity = Vector3.zero;
                rbody.angularVelocity = new Vector3(0f, turnSpeed, 0f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (state)
        {
            case State.FORWARD:
                SetState(State.ROTATE);
                break;
            case State.ROTATE:
                SetState(State.FORWARD);
                break;
        }
    }

    private void SetState(State newState)
    {
        state = newState;
        lastStateTime = Time.fixedTime;
    }
}
