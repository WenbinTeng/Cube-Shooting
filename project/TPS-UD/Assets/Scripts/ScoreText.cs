using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = GameObject.Find("Player").GetComponent<PlayerController>().getGoal().ToString();
    }
}
