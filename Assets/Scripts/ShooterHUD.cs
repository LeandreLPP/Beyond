using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterHUD : MonoBehaviour {

    public PlayerInputController shooter;
    
    public GameObject ammoTextGameObject;
    public GameObject reloadTextGameObject;

    private Text ammoText;
    private Text reloadText;

    // Use this for initialization
    void Start () {
        ammoText = ammoTextGameObject.GetComponent<Text>();
        reloadText = reloadTextGameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        Firearm firearm = null;

        if (shooter.Weapon != null && shooter.Weapon is Firearm)
            firearm = shooter.Weapon as Firearm; 

        string text = "No weapon";
        if (firearm != null)
        {
            text = firearm.Ammo + "/" + firearm.magazine + "\n";
            switch (firearm.Firemode)
            {
                case Firearm.FireMode.SemiAuto:
                    text += "Semi-auto";
                    break;
                case Firearm.FireMode.Burst:
                    text += "Burst "+firearm.burstSize;
                    break;
                case Firearm.FireMode.Automatic:
                    text += "Automatic";
                    break;
            }
        }
        ammoText.text = text;

        if (firearm.Reloading)
        {
            reloadText.enabled = true;
            var time = Mathf.Floor(firearm.ReloadRemaining * 10) / 10;
            reloadText.text = "" + time;
        }
        else
        {
            reloadText.enabled = false;
        }
    }
}
