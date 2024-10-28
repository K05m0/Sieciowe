#if UNITY_EDITOR
// Dodaje przycisk w inspektorze, który ręcznie uruchamia skalowanie czcionek
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextMeshProAutoScaler))]
public class TextMeshProAutoScalerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TextMeshProAutoScaler scaler = (TextMeshProAutoScaler)target;
        if (GUILayout.Button("Dostosuj rozmiary czcionek"))
        {
            scaler.AdjustFontSize();
        }
    }
}
#endif