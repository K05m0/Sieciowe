using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TreeScript : MonoBehaviour
{
    public Vector3Value playerPosition;

    void Start()
    {
        if (transform.position.x < 0)
        {
            float rotation = Random.Range(-100, -80);
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
        else
        {
            float rotation = Random.Range(100, 80);
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        float scale = Random.Range(1, 2);
        transform.localScale = Vector3.one * scale;



    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (distance > 40)
        {
            Destroy(gameObject);

        }
    }
}
