using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager1 : MonoBehaviour
{
    public playerhealth hp;
    public int health = 3;
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;
 
    
    
    void Update()
    {
        /*
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < hp.value; i++)
        {

            hearts[i].sprite = fullHeart;
        }
        */
        

    }
}