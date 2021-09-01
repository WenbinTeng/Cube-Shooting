using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealSlider : MonoBehaviour
{
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Slider>().value = Player.GetComponent<PlayerController>().getHealRate();
    }
}
