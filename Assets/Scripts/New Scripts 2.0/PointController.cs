using UnityEngine;

public class PointController : MonoBehaviour
{
    public float points = 0; // Total points
    public float pointsPerSecond = 1f; // Points awarded per second based on segment speed
    public SegmentController segmentController; // Reference to the SegmentController

    private void Start()
    {
        // Ensure we have a reference to SegmentController
        if (segmentController == null)
        {
            segmentController = FindObjectOfType<SegmentController>();
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(points);
        GetPoints();
    }

    private void Update()
    {
        // Calculate points based on segment speed
        if (segmentController != null)
        {
            points += segmentController.CurrSegmentSpeed / segmentController.maxSegmentSpeed * pointsPerSecond;
        }
    }

    public float GetPoints()
    {
        return points; // Method to retrieve current points
    }
}
