using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAttackRing : MonoBehaviour {

    public DebugAttackDummy dummy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dummy.target)
            dummy.targeting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == dummy.target)
            dummy.targeting = false;
    }
}
