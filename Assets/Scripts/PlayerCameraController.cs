using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    #region Settings
    public new Camera camera;

    public float minimumDistance = 0f;
    public float maximumDistance = 10f;

    public float thirdPersonOffsetSide = 1f;
    public float thirdPersonOffsetHeight = 10f;

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
        //ray = new Ray(); // Debug
    }

    /*Ray ray;
    Vector3 offsetPosGizmos;
    Vector3 dirGizmos;
    private void OnDrawGizmos() // Debug
    {
        Gizmos.DrawSphere(offsetPosGizmos, 0.1f);
        Gizmos.DrawCube(offsetPosGizmos + dirGizmos*2, new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawRay(ray);
    }*/

    void Update ()
    {
        // Set basic camera position
        var offsetSide = cameraOnRight ? thirdPersonOffsetSide : -thirdPersonOffsetSide;
        Quaternion rotationUpDown = Quaternion.AngleAxis(CurrentAngle, Vector3.left);
        Vector3 anglePosition = rotationUpDown * (-Vector3.forward);
        
        Vector3 offsetPos = Vector3.right * offsetSide;
        
        // Make sure the camera is not clipping
        RaycastHit hit;
        
        float trueDistance = CurrentDistance;
        var mask = ~(1 << gameObject.layer); // Mask "IgnoreRaycast"
        var globalDir = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * anglePosition;
        if(Physics.Raycast(transform.position + offsetPos, globalDir, out hit, CurrentDistance, mask))
            trueDistance = hit.distance;

        // Correctly position the camera
        Vector3 positionCamera = (anglePosition.normalized * trueDistance) + offsetPos;
        camera.transform.localPosition = positionCamera;

        // Set camera angle
        var heightAngle = -CurrentAngle - thirdPersonOffsetHeight;
        camera.transform.localEulerAngles = new Vector3(heightAngle, 0, 0);

        // Set FOV
        var diff = FieldOfView - camera.fieldOfView;
        camera.fieldOfView += diff * Time.deltaTime * fovChangingSpeed;
    }
}
