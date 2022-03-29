using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HaltMenu : MonoBehaviour
{
    public GameObject haltMenu;
    public GameObject ammoSlider;
    public GameObject healSlider;
    public GameObject goalSlider;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HaltFunc();
        }
    }

    public void ResumeFunc()
    {
        Time.timeScale = 1f;
        haltMenu.SetActive(false);
        ammoSlider.SetActive(true);
        healSlider.SetActive(true);
        goalSlider.SetActive(true);
    }

    public void HaltFunc()
    {
        Time.timeScale = 0f;
        haltMenu.SetActive(true);
        ammoSlider.SetActive(false);
        healSlider.SetActive(false);
        goalSlider.SetActive(false);
    }

    public void MenuFunc()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitFunc()
    {
        Time.timeScale = 0f;
        Application.Quit();
    }
}
