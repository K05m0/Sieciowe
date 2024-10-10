using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransporter : MonoBehaviour
{
    [SerializeField] private Transform cameraTransporter;
    private float transporterXposition;

    private void Awake()
    {
        transporterXposition = cameraTransporter.position.x;
    }

    private void Update()
    {
        cameraTransporter.position = new Vector3(transporterXposition, transform.position.y, transform.position.z);
    }
}
