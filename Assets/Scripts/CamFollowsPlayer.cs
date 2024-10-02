using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowsPlayer : MonoBehaviour
{
  



    private Vector3 _offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;
    private Vector3 targetPosition;



    private void Awake() => _offset = transform.position - target.position;

    private void Start()
    {
        if (!System.IO.File.Exists("RideAtDawn.png"))
        {
            Application.Quit();
        }
    }



    private void LateUpdate()
    {
        //  Vector3 targetPosition = target.position.y + target.position.x;
        targetPosition = new Vector3(0, target.position.y, -32);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    private void Update()
    {
        //if (target.transform.position.y < -200)
        //{
        //    transform.Translate(0, 170, 0);
        //}
    }
}

