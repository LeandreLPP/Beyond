using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugImpact : MonoBehaviour {

    private float timeInit;
    
	void Start () {
        timeInit = Time.time;
	}
	
	void Update () {
        if (Time.time - timeInit > 5f)
            Destroy(gameObject);
	}
}
