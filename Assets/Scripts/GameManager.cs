using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public GameObject gameoverUI;

    static int curCoin = 0;
    public Text coin;
    public Text finalCoin;

    public GameObject pauseMenu;
    public GameObject winMenu;

    private int originCoin;

    // Start is called before the first frame update
    void Start()
    {
        originCoin = curCoin;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        coin.text = ": " + curCoin.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        Reset();
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        Reset();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static void GameOver(bool dead)
    {
        if (dead)
        {
            GameObject.FindGameObjectWithTag("Music").SendMessage("Be");
            instance.gameoverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public static void AddCoin()
    {
        curCoin++;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        Reset();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public static void Win()
    {
        GameObject.FindGameObjectWithTag("Music").SendMessage("Win");
        instance.winMenu.SetActive(true);
        instance.finalCoin.text = "You get " + curCoin.ToString() + " coins in this turn";
        Time.timeScale = 0f;
    }

    public static void Drop()
    {
        curCoin = instance.originCoin;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Reset()
    {
        HealthSystem.Reset();
        curCoin = 0;
    }
}
