using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NewPlayerMovement;


public class SegmentController : MonoBehaviour
{
    public NewPlayerMovement player;
    public Segment[] SegmentsPrefabs;
    public Segment FirstSegment;
    public Segment SecondSegment;
    public Queue<Segment> SpawnedSegments = new Queue<Segment>();
    public int MaxSegmentsSpawns = 5;
    public float distanceBetweenSegments = 44f;

    public float CurrSegmentSpeed;  // Speed at which segments move up
    public float maxSegmentSpeed = 20f; // Maximum speed limit

    public float BaseAcceleration;
    public float MaxAcceleration;
    public float accelerationIcreaseMultiplayer;
    public float CurrAcceleration = 0.5f;// Amount by which speed increases over time

    private bool isStop = false;

    private void OnEnable()
    {
        OnPlayerDeath += StopSegmentMovement;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= StopSegmentMovement;

    }
    private void StopSegmentMovement()
    {
        CurrSegmentSpeed = 0;
        CurrAcceleration = 0;
        isStop = true;
    }



    private void Awake()
    {
        CurrAcceleration = BaseAcceleration;
        SpawnedSegments.Enqueue(FirstSegment);
        SpawnedSegments.Enqueue(SecondSegment);
        for (int i = 0; i < MaxSegmentsSpawns - 1; i++)
        {
            AddSegments();
        }
    }

    private void Start() // Nowa metoda Start
    {
        StartCoroutine(GenerateSegments()); // Uruchomienie coroutine
    }

    private void Update()
    {
        if (isStop)
        { return; }
        IncreaseSegmentSpeed(); // Zwiększ prędkość w każdej klatce
        MoveSegments();
    }

    private void IncreaseSegmentSpeed()
    {
        CurrSegmentSpeed += CurrAcceleration * Time.deltaTime;

        if (!player.isSlide)
            CurrAcceleration += Time.deltaTime * accelerationIcreaseMultiplayer;
        if (player.isSlide)
            CurrAcceleration -= Time.deltaTime * accelerationIcreaseMultiplayer * 0.2f;

        if (CurrSegmentSpeed > maxSegmentSpeed)
        {
            CurrSegmentSpeed = maxSegmentSpeed;
        }

        if (CurrAcceleration > MaxAcceleration)
        {
            CurrAcceleration = MaxAcceleration;
        }
        if (CurrAcceleration <= BaseAcceleration)
        {
            CurrAcceleration = BaseAcceleration;
        }
    }

    private void MoveSegments()
    {
        // Move each segment up
        foreach (var segment in SpawnedSegments)
        {
            segment.transform.position += Vector3.up * CurrSegmentSpeed * Time.deltaTime;
        }
    }

    private IEnumerator GenerateSegments()
    {
        while (true) // Pętla nieskończona, aby generować segmenty
        {
            if (SpawnedSegments.Count < MaxSegmentsSpawns)
            {
                AddSegments(); // Dodawanie segmentów
            }
            yield return new WaitForSeconds(1f); // Czas w sekundach pomiędzy generowaniem
        }
    }

    public void AddSegments()
    {
        if (SpawnedSegments.Count >= MaxSegmentsSpawns) return; // Unikaj dodawania, jeśli jest za dużo segmentów

        Vector3 pos = new Vector3(0, GetLastSegment().transform.position.y - distanceBetweenSegments, 0);
        Segment newSegment = Instantiate(
            SegmentsPrefabs[Random.Range(0, SegmentsPrefabs.Length)],
            pos, Quaternion.identity
        );
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

        if (index >= 0 && index < SpawnedSegments.Count && index >= Mathf.Round(MaxSegmentsSpawns / 2))
        {
            RemoveSegment();
            AddSegments();
        }
    }

    public Segment GetLastSegment()
    {
        if (SpawnedSegments.Count == 0)
        {
            return null; // Lub rzuć wyjątek, jeśli nie chcesz zwracać null
        }

        // Przekonwertuj kolejkę na tablicę
        Segment[] segmentsArray = SpawnedSegments.ToArray();

        // Zwróć ostatni element
        return segmentsArray[segmentsArray.Length - 1];
    }
}
