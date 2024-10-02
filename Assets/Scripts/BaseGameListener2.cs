using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BaseGameListener2 : MonoBehaviour, IGameEventListener
{
    public GameEvent gameEventToSubscribe;
    //public UnityEvent response;
    public playerhealth hp;
    public TextMeshProUGUI text;
    public int points = 0;
    

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    bool isGameOver = false;

    [SerializeField] private GameObject gameOverPanel;
    private void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Dead zmiana sceny");
            gameOverPanel.SetActive(false);

            SceneManager.LoadScene(0);
        }
    }
    private void OnEnable()
    {
        gameEventToSubscribe.RegisterListener(this);

    }
    private void OnDisable()
    {
        gameEventToSubscribe.UnregisterListener(this);
    }
    public void Notify()
    {

        StartCoroutine(MakeRedForOneSecond());
        if (hp.value >= 0)
        {
            for (int i = 0; i < 3 - hp.value; i++)
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private void Awake()
    {
        //textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        // textMeshProUGUI.text = hp.value.ToString();

        gameOverPanel.SetActive(false);
        foreach (Image img in hearts)
        {
            img.sprite = fullHeart;

        }
        hp.value = 3;
        //textMeshProUGUI.text = points.ToString();
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "floor")
        {

            StartCoroutine(countpoints());
        }
    }
    private IEnumerator countpoints()
    {
        yield return new WaitForSecondsRealtime(1f);
        points++;
        //textMeshProUGUI.text = points.ToString();
    }
    private IEnumerator MakeRedForOneSecond()
    {
        /*
        textMeshProUGUI.color = Color.red;
        textMeshProUGUI.text = hp.value.ToString();
        yield return new WaitForSecondsRealtime(1f);
        textMeshProUGUI.color =Color.white;
        */

        if (hp.value < 1)
        {
            yield return new WaitForSecondsRealtime(1f);
            gameOverPanel.SetActive(true);
            isGameOver = true;
            
           //text.rectTransform.position = new Vector3(1534, 764, -740);
        }

    }
    //private IEnumerator killedBarrier() 
    //{ 

    //}

   

}
