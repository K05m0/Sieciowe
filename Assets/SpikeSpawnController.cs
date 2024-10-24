using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawnController : MonoBehaviour
{
    // Lista punktów, w których mogą pojawiać się przeszkody (holdery).
    public List<Transform> spawnPoints;  // Zainicjalizuj te punkty w Inspectorze Unity.

    // Prefab przeszkody, którą będziemy instancjonować.
    public GameObject obstaclePrefab;

    // Zakres losowego interwału.
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 5f;

    // Ilość holderów, w których mogą pojawić się przeszkody na raz (random).
    public int minObstacles = 1;
    public int maxObstacles = 3;

    private void Start()
    {
        // Zainicjuj losowe spawnienie przeszkód.
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Poczekaj losowy czas (interwał).
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            // Losowa ilość przeszkód do wygenerowania.
            int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);

            // Lista dostępnych punktów, w których możemy generować przeszkody.
            List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

            for (int i = 0; i < obstacleCount; i++)
            {
                if (availableSpawnPoints.Count == 0)
                {
                    break; // Nie ma już dostępnych punktów.
                }

                // Losowy wybór punktu z dostępnych.
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];

                // Stwórz przeszkodę na wybranym punkcie.
                Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);

                // Usuń punkt z dostępnych, żeby nie pojawiły się dwie przeszkody w tym samym miejscu.
                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }
}
