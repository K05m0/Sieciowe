using UnityEngine;

public class Segment : MonoBehaviour
{
    // Funkcja, która zostanie wywołana przez dziecko
    public bool IsInSegment;
    public void OnChildTriggerExit(Collider other)
    {
        Debug.Log("Trigger wykryty w dziecku segmentu: " + gameObject.name);
        // Tutaj możesz dodać swoją logikę
        IsInSegment = !IsInSegment;
    }
}
