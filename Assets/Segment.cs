using UnityEngine;

public class Segment : MonoBehaviour
{
    public bool IsInSegment;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInSegment = true;
            Debug.Log("Gracz wszedł w segment: " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInSegment = false;
            Debug.Log("Gracz opuścił segment: " + gameObject.name);
        }
    }
}
