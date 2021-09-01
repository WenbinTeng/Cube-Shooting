using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorMovement : MonoBehaviour
{
    private Vector3 positivePosition;
    private Vector3 negativePosition;

    private float changeTimer;
    private float changeIntvl;

    // Start is called before the first frame update
    void Start()
    {
        positivePosition = new Vector3(5f, 10f, +5f);
        negativePosition = new Vector3(5f, 10f, -5f);

        changeTimer = 5f;
        changeIntvl = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > changeTimer)
        {
            changeTimer = Time.time + changeIntvl;

            if (GameObject.Find("Player").transform.position.z > 0)
            {
                transform.position = negativePosition;
            }
            if (GameObject.Find("Player").transform.position.z < 0)
            {
                transform.position = positivePosition;
            }
        }
    }
}
