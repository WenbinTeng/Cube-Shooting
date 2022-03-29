using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverMenu : MonoBehaviour
{
    public GameObject Player;
    public GameObject overMenu;
    public GameObject ammoSliders;
    public GameObject healSliders;
    public GameObject scoreText;
    public Text scoreInfo;
    private float exitTimer;
    private float exitIntvl;

    private void Start()
    {
        exitTimer = 0f;
        exitIntvl = 5f;
    }

    void Update()
    {
        if (Player.GetComponent<PlayerController>().getHeal() <= 0f)
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

        scoreInfo.text = Player.GetComponent<PlayerController>().getGoal().ToString();
        overMenu.SetActive(true);
        ammoSliders.SetActive(false);
        healSliders.SetActive(false);
        scoreText.SetActive(false);

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
