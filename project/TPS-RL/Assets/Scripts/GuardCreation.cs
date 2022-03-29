using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCreation : MonoBehaviour
{
    public GameObject guardPrefab;

    private float createTimer;
    private float createIntvl;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 0.0f;
        createIntvl = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        createTimer += Time.deltaTime;

        if (createTimer > createIntvl)
        {
            createTimer = 0f;

            Vector3 createPosition = transform.position + new Vector3(
                0f,
                0f,
                Random.Range(-5f, 5f)
            );
            Quaternion createRotation = Quaternion.Euler(
                Random.Range(-90f, 90f),
                Random.Range(-90f, 90f),
                Random.Range(-90f, 90f)
            );

            Instantiate(guardPrefab, createPosition, createRotation);
        }
    }
}
