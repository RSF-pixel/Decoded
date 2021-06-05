using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject creditsPanel;
    [Header("Sound")]
    public AudioSource sound;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        sound.Play();
    }

    public void StartGame()
    {
        Invoke("LoadGame", 1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void Return()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
