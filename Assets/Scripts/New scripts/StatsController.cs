using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public List<Stat> AllObjectStats = new List<Stat>();

    public void AddNewStat(E_StatType statName, float currValue, float maxValue)
    {
        Stat newStat = new Stat
        {
            StatType = statName,
            StatCurrValue = currValue,
            StatMaxValue = maxValue
        };

        AllObjectStats.Add(newStat);
    }
}