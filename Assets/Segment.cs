using UnityEngine;

public class Segment : MonoBehaviour
{
    public bool IsInSegment;
    private SegmentController controller;

    private void Awake()
    {
        controller = FindObjectOfType<SegmentController>();
    }

    public void OnChildTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            controller.CheckSegments(this);
        }
    }
}
