using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform car;
    public Camera thirdPersonCamera;
    public Camera firstPersonCamera;
    private bool isFirstPerson = false;

    void Start()
    {
        SwitchCameraMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            SwitchCameraMode();
        }
    }

    void SwitchCameraMode()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);
    }
}
