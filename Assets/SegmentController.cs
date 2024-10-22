using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    public Segment[] SegmentsPrefabs;
    public Dictionary<Segment, bool> SpawnedSegments; // segments count + chcek you stay in this segment
    public int MaxSegmentsSpawns;

    public void AddSegments()
    {

    }

}
