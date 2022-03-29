using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCreation : MonoBehaviour
{
    public Transform Player;

    public GameObject guardPrefab;

    private float createTimer;
    private float createIntvl;

    private int createCnt;
    private int createNmb;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 0f;
        createIntvl = 2f;

        createCnt = 0;
        createNmb = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > createTimer)
        {
            createTimer = Time.time + createIntvl;

            createCnt++;

            if (createCnt < createNmb)
            {
                createTimer = 0;
            }

            Vector3 guardPosition = new Vector3(
                Random.Range(-20f, 20f),
                Random.Range(  5f, 10f),
                Random.Range(-20f, 20f)
            );

            Vector3 createPosition = Player.position + guardPosition;

            if (createPosition.z < -20) createPosition.z = -20 + guardPrefab.transform.localScale.z;
            if (createPosition.x < -20) createPosition.x = -20 + guardPrefab.transform.localScale.x;
            if (createPosition.z > 20) createPosition.z = 20 - guardPrefab.transform.localScale.z;
            if (createPosition.x > 20) createPosition.x = 20 - guardPrefab.transform.localScale.x;

            Instantiate(guardPrefab, createPosition, Quaternion.Euler(0, 0, 0));
        }
    }
}
