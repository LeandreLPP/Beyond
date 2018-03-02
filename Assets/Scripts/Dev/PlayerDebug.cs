using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour {
    
    // Debug
    public Material materialGround;
    public Material normal;

    MovementController movementController;
    TextMesh text;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        text = GetComponentInChildren<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        // Debug
        GetComponentInChildren<MeshRenderer>().material = movementController.Grounded ? materialGround : normal;

        string allure = "";
        switch (movementController.MovementState)
        {
            case MovementState.Stopped:
                allure = "Stopped";
                break;
            case MovementState.Walking:
                allure = "Walking";
                break;
            case MovementState.Running:
                allure = "Running";
                break;
            case MovementState.Sprinting:
                allure = "Sprinting";
                break;
        }
        string texteuh = allure;
        
        if (movementController.Crouching)
            texteuh += "\n" + "Crouch";
        if (movementController.Platform)
            texteuh += "\n" + "Platform";

        text.text = texteuh;
    }
}
