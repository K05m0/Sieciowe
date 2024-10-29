using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;



    private void OnEnable()
    {
        NewPlayerMovement.OnPlayerDeath += ShowHideGameOver;

    }
    private void OnDisable()
    {
        NewPlayerMovement.OnPlayerDeath -= ShowHideGameOver;
    }
    private void Awake()
    {
        GameOverScreen.SetActive(false);
    }

    private void ShowHideGameOver()
    {
        if(GameOverScreen.active)
        {
            GameOverScreen.SetActive(false);
        }
        else
        {
            GameOverScreen.SetActive(true);
        }
    }
}
