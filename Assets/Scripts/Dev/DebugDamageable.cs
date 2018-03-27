using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDamageable : Damageable {

    public Material materialDamage;
    public float delay = 0.5f;

    private Material baseMaterial;
    private float lastChange;
    private bool changed;
    private MeshRenderer meshRenderer;

    // Use this for initialization
    void Start () {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (!meshRenderer)
            meshRenderer = GetComponent<MeshRenderer>();
        changed = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(changed && (Time.time - lastChange) >= delay)
        {
            meshRenderer.material = baseMaterial;
            changed = false;
        }
    }

    public override void TakeDamages(float damageAmount, AWeapon source)
    {
        if(!changed)
        {
            baseMaterial = meshRenderer.material;
            meshRenderer.material = materialDamage;
            changed = true;
        }
        lastChange = Time.time;
    }
}
