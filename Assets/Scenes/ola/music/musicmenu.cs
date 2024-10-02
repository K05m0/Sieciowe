using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicmenu : MonoBehaviour
{
    private static musicmenu backgroundmusic;

    void Awake()
    {
        if (backgroundmusic ==null)
        {
            backgroundmusic = this;
            DontDestroyOnLoad(backgroundmusic);

            
        }


        else
        {
            Destroy(gameObject);
        }
    }
  
}
