using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Transform Player;
    private Vector3 initPosition;
    private Vector3 goalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        initPosition = transform.position - Player.position;
        goalPosition = transform.position - Player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        goalPosition = Vector3.Lerp(transform.position, Player.position + initPosition, Time.deltaTime * 5);

        if (Vector3.Distance(transform.position, goalPosition) > 0.01f)
        {
            transform.position = goalPosition;
        }
    }
}
