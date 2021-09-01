using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void PlayGameLight()
    {
        SceneManager.LoadScene("Light");
    }

    public void PlayGameNight()
    {
        SceneManager.LoadScene("Night");
    }
}
