using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    private Vector3 initPosition;
    private Vector3 goalPosition;

    private float moveTimer;
    private float moveIntvl;
    private float moveValue;

    public enum WallType
    {
        Hor_R,
        Hor_L,
        Ver_U,
        Ver_D,
    }
    public WallType type;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        goalPosition = transform.position;

        moveTimer = 10f;
        moveIntvl = 10f;
        moveValue = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTimer > moveIntvl)
        {
            moveTimer = 0;

            if (Random.Range(0, 1f) >= 0.5f)
            {
                initPosition = transform.position;

                switch (type)
                {
                    case WallType.Hor_L:
                        goalPosition = initPosition + new Vector3(moveValue, 0, 0);
                        break;
                    case WallType.Hor_R:
                        goalPosition = initPosition - new Vector3(moveValue, 0, 0);
                        break;
                    case WallType.Ver_U:
                        goalPosition = initPosition - new Vector3(0, 0, moveValue);
                        break;
                    case WallType.Ver_D:
                        goalPosition = initPosition + new Vector3(0, 0, moveValue);
                        break;
                }

                moveValue *= -1;
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
        }

        transform.position = Vector3.Distance(transform.position, goalPosition) > 0.1f ? transform.position + (goalPosition - initPosition) * Time.deltaTime / 2 : goalPosition;
    }
}
