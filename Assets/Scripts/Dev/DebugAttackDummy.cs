using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAttackDummy : MonoBehaviour {

    public bool targeting;
    public GameObject target;
    public GameObject ring;
    
	// Update is called once per frame
	void Update ()
    {
        GetComponent<MovementController>().MovementState = MovementState.Walking;

        if (target != null)
        {

            transform.LookAt(target.transform);
            if (targeting)
            {
                GetComponent<MovementController>().Direction = target.transform.position - transform.position;
                transform.LookAt(target.transform);
                (GetComponent<ACarrier>().Weapon as MeleeWeapon).QuickStrike();
            }
            else
            {
                GetComponent<MovementController>().Direction = ring.transform.position - transform.position;

                if ((transform.position - ring.transform.position).magnitude < 1f)
                {
                    transform.position = ring.transform.position;
                    transform.eulerAngles = Vector3.zero;
                }
            }
        }
    }
}
