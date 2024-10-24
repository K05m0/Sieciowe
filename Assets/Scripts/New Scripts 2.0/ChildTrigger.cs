using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Szukamy komponentu Segment na obiekcie rodzica
            Segment parentSegment = GetComponentInParent<Segment>();
            if (parentSegment != null)
            {
                // Wywołaj funkcję w skrypcie rodzica
                parentSegment.OnChildTriggerExit(other);
            }
        }
    }
}
