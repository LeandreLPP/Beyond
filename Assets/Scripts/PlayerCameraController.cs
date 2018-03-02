using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    #region Settings
    public new Camera camera;

    public float minimumDistance = 0f;
    public float maximumDistance = 10f;
    public float thirdPersonOffsetAngle = 10f;

    public float fovChangingSpeed = 10f;

    public bool cameraOnRight = true;
    #endregion

    private float currentDistance;
    public float CurrentDistance {
        get
        {
            return currentDistance;
        }

        set
        {
            currentDistance = Mathf.Max(minimumDistance, Mathf.Min(maximumDistance, value));
        }
    }
    public float CurrentAngle { get; set; }
    public float FieldOfView { get; set; }

    private void Start()
    {
        camera.transform.SetParent(transform);
        CurrentDistance = maximumDistance;
        FieldOfView = camera.fieldOfView;
    }
    
    void Update ()
    {
        // Set camera position
        var offsetAngle = cameraOnRight ? thirdPersonOffsetAngle : -thirdPersonOffsetAngle;
        Quaternion rotationOffset = Quaternion.AngleAxis(offsetAngle, Vector3.up);
        Quaternion rotationUpDown = Quaternion.AngleAxis(CurrentAngle, Vector3.left);
        Vector3 anglePosition = (rotationUpDown * rotationOffset) * (-Vector3.forward);
        
        Vector3 positionCamera = anglePosition.normalized * CurrentDistance;

        camera.transform.localPosition = positionCamera;

        // Set camera angle
        var turnAngle = offsetAngle / 2f;
        var heightAngle = -CurrentAngle - 5f;
        camera.transform.localEulerAngles = new Vector3(heightAngle, turnAngle, 0);

        // Set FOV
        var diff = FieldOfView - camera.fieldOfView;
        camera.fieldOfView += diff * Time.deltaTime * fovChangingSpeed;
    }
}
