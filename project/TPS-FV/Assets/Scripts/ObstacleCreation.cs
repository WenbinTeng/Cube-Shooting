using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreation : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int mapZ;
    public int mapX;

    // Start is called before the first frame update
    void Start()
    {
        mapZ /= 2;
        mapX /= 2;

        Invoke("CreateObstacle", 0f);
        Invoke("CreateObstacle", 1f);
        Invoke("CreateObstacle", 2f);
        Invoke("CreateObstacle", 3f);
        Invoke("CreateObstacle", 4f);
        Invoke("CreateObstacle", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateObstacle()
    {
        GameObject ob = Instantiate(obstaclePrefab, transform.position, transform.rotation);
        ob.transform.Translate(
            Random.Range(-20f, +20f),
            Random.Range(+10f, +30f),
            Random.Range(-20f, +20f)
            );
        ob.transform.Rotate(
            Random.Range(0f, 180f),
            Random.Range(0f, 180f),
            Random.Range(0f, 180f)
            );
        ob.transform.localScale = new Vector3(
            Random.Range(1f, 10f),
            Random.Range(1f, 10f),
            Random.Range(1f, 10f)
            );
    }
}
