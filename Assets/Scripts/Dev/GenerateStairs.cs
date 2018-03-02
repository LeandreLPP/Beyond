using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStairs : MonoBehaviour {

    public float step = 0.2f;
    public int number = 10;
    public GameObject cubePrefab;

	// Use this for initialization
	void Start ()
    {
		for(int i = 0; i<number; i++)
        {
            var pos = transform.position + new Vector3(i, (i * step) - 0.5f);
            Instantiate<GameObject>(cubePrefab, pos, new Quaternion());
        }
	}

}
