using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalSlider : MonoBehaviour
{
    public GameObject Player;
    private float preValue;
    private float curValue;
    private float changeTimer;
    private float changeIntvl;
    public Text scoreText;

    // Start is called before the first frame update
    private void Start()
    {
        preValue = 0f;
        curValue = 0f;
        changeTimer = 0f;
        changeIntvl = 1f;
        gameObject.GetComponent<Slider>().value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        curValue = Player.GetComponent<PlayerController>().getGoalRate();

        if (changeTimer < changeIntvl)
        {
            changeTimer += Time.deltaTime;

            if (Mathf.Abs(preValue - curValue) > 1e-5)
            {
                preValue += (curValue - preValue) / (changeIntvl - changeTimer) * Time.deltaTime;
            }

            gameObject.GetComponent<Slider>().value = preValue;
        }
        else
        {
            changeTimer = 0f;
        }

        scoreText.text = Player.GetComponent<PlayerController>().getGoalAll().ToString();
    }
}
