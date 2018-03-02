using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerAnimatorController : MonoBehaviour {

    MovementController movementController;
    Animator animator;
    
    void Start () {
        movementController = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        int speed = 0;
        switch (movementController.MovementState)
        {
            case MovementState.Stopped:
                speed = 0;
                break;
            case MovementState.Walking:
                speed = 1;
                break;
            case MovementState.Running:
                speed = 2;
                break;
            case MovementState.Sprinting:
                speed = 3;
                break;
        }
        animator.SetInteger("speed", speed);
        animator.SetBool("crouched", movementController.Crouching);
    }
}
