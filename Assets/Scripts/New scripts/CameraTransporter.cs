using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransporter : MonoBehaviour
{
    [SerializeField] private Transform cameraTransporter;
    private float transporterXposition;
    private float transporterYposition;

    private void Awake()
    {
        transporterXposition = cameraTransporter.position.x;
        transporterYposition = cameraTransporter.position.y;
    }

    private void Update()
    {
        cameraTransporter.position = new Vector3(transporterXposition, transporterYposition, transform.position.z);
    }
}
