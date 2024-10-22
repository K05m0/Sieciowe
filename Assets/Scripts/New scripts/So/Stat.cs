using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [Serialize] public E_StatType StatType;
    [Serialize] public float StatCurrValue;
    [Serialize] public float StatMaxValue;
}
