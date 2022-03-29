using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverMenu : MonoBehaviour
{
    public GameObject Player;
    public GameObject overMenu;
    public GameObject ammoSlider;
    public GameObject healSlider;
    public GameObject goalSlider;
    public Text goalText;
    private float exitTimer;
    private float exitIntvl;

    private void Start()
    {
        exitTimer = 0f;
        exitIntvl = 5f;
    }

    void Update()
    {
        if (Player.GetComponent<PlayerController>().getHealRate() <= 0f)
        {
            OverFunc();
        }
    }

    public void OverFunc()
    {
        if (exitTimer > exitIntvl)
        {
            exitTimer = 0;
            exitIntvl = 0;

            SceneManager.LoadScene("Menu");
        }
        else
        {
            exitTimer += Time.deltaTime;
        }

        Camera.main.GetComponent<CamShake>().StartFunc(0.02f, 1f);

        goalText.text = Player.GetComponent<PlayerController>().getGoalAll().ToString();
        overMenu.SetActive(true);
        ammoSlider.SetActive(false);
        healSlider.SetActive(false);
        goalSlider.SetActive(false);

        GetComponent<HaltMenu>().enabled = false;
    }

    public void MenuFunc()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitFunc()
    {
        Application.Quit();
    }
}
