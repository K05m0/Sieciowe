using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    public Segment[] SegmentsPrefabs;
    public Queue<Segment> SpawnedSegments = new Queue<Segment>(); // segments count + chcek you stay in this segment
    public int MaxSegmentsSpawns = 5;
    public float distanceBetweenSegments = 44;
    private int CreatedSegmentsCount = 1;


    private void Awake()
    {
        for(int i = 0; i < MaxSegmentsSpawns; i++) 
        {
            AddSegments();
        }
    }

    public void AddSegments()
    {
        Vector3 pos = new Vector3(0, -distanceBetweenSegments * CreatedSegmentsCount, 0);
        Segment newSegment = Instantiate(
            SegmentsPrefabs[Random.Range(0, SegmentsPrefabs.Length)],
            pos, Quaternion.identity);

        CreatedSegmentsCount++;
        SpawnedSegments.Enqueue(newSegment);
    }

    public void RemoveSegment()
    {
        var segment = SpawnedSegments.Dequeue();
        Destroy(segment.gameObject);
    }

    public void CheckSegments(Segment segment)
    {
        List<Segment> list = new List<Segment>(SpawnedSegments);
        int index = list.IndexOf(segment);
        Debug.Log(SpawnedSegments.Count);
        Debug.Log($"Leaved Segment have index {index}");

        // Zmiana warunków na większe bezpieczeństwo
        if (index >= 0 && index < SpawnedSegments.Count && index >= Mathf.Round( MaxSegmentsSpawns/2))
        {
            Debug.Log($"spawn on index {index}");
            RemoveSegment();
            AddSegments();
        }
    }
}
