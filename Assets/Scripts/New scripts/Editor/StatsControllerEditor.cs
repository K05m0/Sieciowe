using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatsController), true)]
public class StatsControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StatsController objectStats = (StatsController)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Object Stats", EditorStyles.boldLabel);

        for (int i = 0; i < objectStats.AllObjectStats.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            objectStats.AllObjectStats[i].StatType = (E_StatType)EditorGUILayout.EnumPopup("Stats Type", objectStats.AllObjectStats[i].StatType);
            objectStats.AllObjectStats[i].StatCurrValue = EditorGUILayout.FloatField("Curr Stat Value", objectStats.AllObjectStats[i].StatCurrValue);
            objectStats.AllObjectStats[i].StatMaxValue = EditorGUILayout.FloatField("Max Stat Value", objectStats.AllObjectStats[i].StatMaxValue);

            // Przycisk do usuwania statystyki
            if (GUILayout.Button("Remove Stat"))
            {
                objectStats.AllObjectStats.RemoveAt(i);
                break; // Przerywamy pętlę, by uniknąć problemów przy edycji listy
            }

            EditorGUILayout.EndVertical();
        }

        // Dodaj nową statystykę
        if (GUILayout.Button("Dodaj nową statystykę"))
        {
            objectStats.AllObjectStats.Add(new Stat());
        }

        // Zapisz zmiany w skrypcie (jeśli zmodyfikowano cokolwiek)
        if (GUI.changed)
        {
            EditorUtility.SetDirty(objectStats);
        }
    }
}
