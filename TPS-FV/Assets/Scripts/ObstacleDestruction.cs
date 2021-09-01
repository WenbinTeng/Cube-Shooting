using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestruction : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -50)
        {
            Destroy(gameObject);
        }
    }
}
