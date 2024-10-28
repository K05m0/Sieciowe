using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NewPlayerMovement;

public class PointController : MonoBehaviour
{
    public float points = 0; // Total points
    public float pointsPerSecond = 1f; // Points awarded per second based on segment speed
    public SegmentController segmentController; // Reference to the SegmentController
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI hightPointText;

    private void OnEnable()
    {
        hightPointText.text = $"High Score: {PlayerPrefs.GetFloat("HighScore", 0f).ToString("F2")}";
        OnPlayerDeath += SaveHighScore;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= SaveHighScore;
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        // Pobieramy obecnie zapisany najlepszy wynik
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        float points = GetPoints();

        // Sprawdzamy, czy obecny wynik jest lepszy niż poprzedni zapisany wynik
        if (points > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", points);  // Zapisujemy nowy najlepszy wynik
            PlayerPrefs.Save();  // Zapisujemy zmiany w PlayerPrefs
            Debug.Log("Nowy najlepszy wynik zapisany: " + points);
        }
    }

    public float LoadHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        Debug.Log("Odczytano najlepszy wynik: " + highScore);
        return highScore;
    }

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
        pointText.text = GetPoints().ToString("F2");
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
