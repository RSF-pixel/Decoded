using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public static bool showPanel = false;
    public static string showText = "";
    [SerializeField] GameObject panel;
    [SerializeField] Text text;

    void Update()
    {
        panel.SetActive(showPanel);
        text.text = showText;
        Cursor.lockState = !showPanel ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = showPanel;
        Time.timeScale = System.Convert.ToSingle(!showPanel);
    }

    public void Restart()
    {
        showPanel = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
