using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public float playerSpeed = 5;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(playerSpeed, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.position += new Vector3(playerSpeed, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.W))
        {


            transform.position += new Vector3(0f, 0f, playerSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0f, 0f, playerSpeed);
        }
        else
        {
            
        }

    }

}