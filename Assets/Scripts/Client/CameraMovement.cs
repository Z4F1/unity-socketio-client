using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    float lastRot = 0;

    // Update is called once per frame
    void LateUpdate () {
        transform.position = target.position - offset;

        float smooth = Mathf.Lerp(lastRot, target.rotation.eulerAngles.y, smoothSpeed);

        transform.RotateAround(target.position, Vector3.up, smooth);
        transform.LookAt(target.position);

        lastRot = transform.rotation.eulerAngles.y;
    }
}
