using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Delete : MonoBehaviour
{
    public Vector3Value playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (distance > 320)
        {
            Destroy(gameObject);

        }
    }
}
