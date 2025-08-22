using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Transform _cameraTransform;
    private Transform _robotTransform;

    public float offset;

    private void Awake()
    {
        _cameraTransform = this.gameObject.transform;
        _robotTransform = GameObject.Find("Robot").transform;
    }

    private void FixedUpdate()
    {
        _cameraTransform.position = _robotTransform.position + new Vector3(0, offset ,0);
        _cameraTransform.rotation = _robotTransform.rotation;
    }
}
