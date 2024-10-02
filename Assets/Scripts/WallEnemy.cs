using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEnemy : MonoBehaviour
{
    public Vector3Value playerPosition;
    private Vector3 startPosition;
    public int health = 1;


    void Start()
    {
        transform.DOMoveY(20f, 2f).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutExpo);


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

