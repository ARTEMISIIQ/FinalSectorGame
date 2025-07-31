using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI bestTime1;
    public TextMeshProUGUI bestTime2;
    public TextMeshProUGUI bestTime3;
    public TextMeshProUGUI bestTime4;
    public TextMeshProUGUI bestTime5;
    public TextMeshProUGUI bestTime6;

    public void PlayGame(int n)
    {
        SceneManager.LoadScene(n + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Update()
    {
        if (PlayerPrefs.GetFloat("2") != 0)
        {
            bestTime1.text = PlayerPrefs.GetFloat("2") + " secs";
        }
        if (PlayerPrefs.GetFloat("3") != 0)
        {
            bestTime2.text = PlayerPrefs.GetFloat("3") + " secs";
        }
        if (PlayerPrefs.GetFloat("4") != 0)
        {
            bestTime3.text = PlayerPrefs.GetFloat("4") + " secs";
        }
        if (PlayerPrefs.GetFloat("5") != 0)
        {
            bestTime4.text = PlayerPrefs.GetFloat("5") + " secs";
        }
        if (PlayerPrefs.GetFloat("6") != 0)
        {
            bestTime5.text = PlayerPrefs.GetFloat("6") + " secs";
        }
    }
}
