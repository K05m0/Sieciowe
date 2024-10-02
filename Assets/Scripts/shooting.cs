using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public Canvas canvas;
    public Camera camera2;

    void Update()
    {


        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        Vector2 mouseOnScreenScaled = (Vector2)camera2.ScreenToViewportPoint(Input.mousePosition);
        

        float angle = AngleBetweenTwoPoints(-positionOnScreen, -mouseOnScreenScaled);

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

      //  Debug.DrawLine(positionOnScreen, mouseOnScreen, Color.green);
      //  Debug.DrawLine(-positionOnScreen, -mouseOnScreenScaled, Color.red);
      //  Debug.Log("mouseOnScreen :" + mouseOnScreen + ", Scaled: " + mouseOnScreenScaled  + " positionOnScreen :" + positionOnScreen);
      //  Debug.Log("ScaleFactor = " + canvas.scaleFactor);

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(-a.y - -b.y, a.x - b.x) * -Mathf.Rad2Deg;
    }
}