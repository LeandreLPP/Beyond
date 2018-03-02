using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovementController>())
            other.GetComponent<MovementController>().Platform = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MovementController>())
            other.GetComponent<MovementController>().Platform = null;
    }
}
