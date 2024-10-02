using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour
{
    public Vector3Value playerPosition;
    private Vector3 startPosition;
    public int health = 1;


    void Start()
    {


    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
        }
    }



    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (distance > 40)
        {
            Destroy(gameObject);

        }


    }
}

