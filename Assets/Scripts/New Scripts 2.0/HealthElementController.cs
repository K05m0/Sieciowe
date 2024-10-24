using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthElementController : MonoBehaviour
{
    [SerializeField] private Image hpImage;
    [SerializeField] private Sprite emptyHp;
    [SerializeField] private Sprite fullHp;

    public void ChangeElementToFull()
    {
        if (hpImage != null)
        {
            hpImage.sprite = fullHp;
        }
    }

    public void ChangeElementToEmpty()
    {
        if (emptyHp != null)
        {
            hpImage.sprite = emptyHp;
        }
    }
}
