using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControler : MonoBehaviour
{
    public GameEvent onPlayerdamage;
    public playerhealth Hp;

    public GameEvent onPlayerdeath;

   

    private void OnMouseDown()
    {
        Hp.value -= 1;
        if (onPlayerdamage != null)
            onPlayerdamage.Fire();
    }
    
}
