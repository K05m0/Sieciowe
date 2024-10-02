using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaBar : MonoBehaviour
{
/*    public Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina;

    public static StaminaBar instance;
    private Coroutine regen;

    private void Awake()
    {
        
    }


    void Start()
    {
        instance = this;
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        

    }

    public void UseStamina(int amount)
    {
        if(currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());
        } 
        else
        {
            //Debug.Log("NOT ENOUGH STAMINA");
        }
    }


    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1.5f);

        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 20;
            staminaBar.value = currentStamina;
            yield return new WaitForSeconds(0.1f);
        }
        regen = null;  
    }
*/
}
