using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    
    
    public float health;

    public Vector3Value playerPosition;
    // Start is called before the first frame update
    void Start()
    {
       int side = Random.Range(0, 2);
       
       if (side == 0)
        {
            
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (distance > 40)
        {
            Destroy(gameObject);
            //idk ale wywala errory tak¿e zostaje w //
        }


        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;

        }
    }
}
