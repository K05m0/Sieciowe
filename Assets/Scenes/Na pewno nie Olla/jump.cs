using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    public int playerSpeed = 5;
    public Rigidbody myRb;
    public float jumpPower;


    private int jumpingCount = 0;

    private void  FixedUpdate()
    {
        myRb.MovePosition(transform.position + new Vector3(
       Input.GetAxis("Horizontal"),
        0f,
        Input.GetAxis("Vertical")
        ) * Time.fixedDeltaTime * playerSpeed);
    }

  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpingCount <= 1)
            Jump();

    }
    private void Jump()
    {
        jumpingCount++;
        myRb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "floor")
        {
            jumpingCount = 0;
           
        }
    }
}
