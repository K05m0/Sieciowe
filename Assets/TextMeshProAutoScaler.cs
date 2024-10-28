using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

[ExecuteAlways]
public class TextMeshProAutoScaler : MonoBehaviour
{
    public List<TextMeshsHolder> TextMeshHolders = new List<TextMeshsHolder>();

    public float minFontSize = 0f;
    public float maxFontSize = 400f;

    private void Awake()
    {
        StartCoroutine(ForceToRefreshScale());
    }

    private IEnumerator ForceToRefreshScale()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        AdjustFontSize();
        Canvas.ForceUpdateCanvases();
    }

    private void OnEnable()
    {
        StartCoroutine(ForceToRefreshScale());
    }

    public void AdjustFontSize()
    {
        foreach (var holder in TextMeshHolders)
        {
            if (!holder.UseCustomSize)
                continue;

            float optimalMaxFontSize = maxFontSize;

            // Znajdź minimalny rozmiar czcionki spośród wszystkich TextMeshPro
            foreach (TextMeshProUGUI tmp in holder.AllTextMesh)
            {
                tmp.enableAutoSizing = true;
                tmp.fontSizeMin = minFontSize;
                tmp.fontSizeMax = maxFontSize;

                tmp.ForceMeshUpdate();
                float fittedFontSize = tmp.fontSize;

                if (fittedFontSize < optimalMaxFontSize)
                {
                    optimalMaxFontSize = fittedFontSize;
                }
            }

            // Jeśli `Dependence` jest zaznaczone, dostosuj skalę względem innego holdera
            if (holder.Dependence && TextMeshHolders[holder.DependOnIndex] != null)
            {
                // Oblicz rozmiar na podstawie DependebleScale
                optimalMaxFontSize = TextMeshHolders[holder.DependOnIndex].GetOptimalFontSize() * holder.DependebleScale;
            }

            foreach (TextMeshProUGUI tmp in holder.AllTextMesh)
            {
                tmp.fontSizeMax = optimalMaxFontSize;
            }
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            AdjustFontSize();
        }
    }

    public void AddTextMeshPro(TextMeshProUGUI tmp)
    {
        foreach (var item in TextMeshHolders)
        {
            if (!item.AllTextMesh.Contains(tmp))
            {
                item.AllTextMesh.Add(tmp);
                AdjustFontSize();
            }
        }
    }
}

[Serializable]
public class TextMeshsHolder
{
    public List<TextMeshProUGUI> AllTextMesh;

    public bool UseCustomSize = true;

    // Nowe pole - jeśli zależność od innego TextMeshsHolder
    public bool Dependence = false;

    // Referencja do innego TextMeshsHolder
    public int DependOnIndex;

    // Parametr w zakresie 0-1, określający o ile mniejsza ma być czcionka w zależności
    [Range(0f, 1f)]
    public float DependebleScale = 1f;

    // Funkcja pomocnicza do zwracania najlepszego rozmiaru czcionki
    public float GetOptimalFontSize()
    {
        float optimalFontSize = float.MaxValue;

        foreach (TextMeshProUGUI tmp in AllTextMesh)
        {
            if (tmp.fontSize < optimalFontSize)
            {
                optimalFontSize = tmp.fontSize;
            }
        }

        return optimalFontSize;
    }
}
