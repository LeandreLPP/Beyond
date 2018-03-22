using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParry : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        (GetComponent<ACarrier>().Weapon as MeleeWeapon).Parry();
	}
}
