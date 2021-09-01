using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCreation : MonoBehaviour
{
    public GameObject flashPrefab;
    private float createTimer;
    private float createIntvl;
    public int mapZ;
    public int mapX;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 0f;
        createIntvl = 10f;
        mapZ /= 2;
        mapX /= 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > createTimer)
        {
            createTimer = Time.time + createIntvl;

            Vector3 flashlPosition = new Vector3(
                Random.Range(-mapX + 5, mapX - 5),
                Random.Range(10f, 30f),
                Random.Range(-mapZ + 5, mapZ - 5)
                );
            Vector3 flashRotation = new Vector3(
                Random.Range(0f, 180f),
                Random.Range(0f, 180f),
                Random.Range(0f, 180f)
                );

            GameObject barrel = Instantiate(flashPrefab, flashlPosition, Quaternion.Euler(flashRotation));
        }
    }
}
