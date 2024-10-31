using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    int _rotationSpeed = 180;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     



            // Rotation on y axis
            // be sure to capitalize Rotate or you'll get errors
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        
    }
}
