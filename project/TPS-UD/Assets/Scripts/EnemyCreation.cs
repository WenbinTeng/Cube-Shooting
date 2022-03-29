using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreation : MonoBehaviour
{
    public Transform Player;

    public GameObject enemyPrefab;

    private float createTimer;
    private float createIntvl;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 10f;
        createIntvl = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > createTimer)
        {
            createTimer = Time.time + createIntvl;

            Vector3 enemyPosition = new Vector3(
                Random.Range(-20f, 20f),
                Random.Range(  5f, 10f),
                Random.Range(-20f, 20f)
            );

            Vector3 createPosition = Player.position + enemyPosition;

            if (createPosition.z < -20) createPosition.z = -20 + enemyPrefab.transform.localScale.z;
            if (createPosition.x < -20) createPosition.x = -20 + enemyPrefab.transform.localScale.x;
            if (createPosition.z > 20) createPosition.z = 20 - enemyPrefab.transform.localScale.z;
            if (createPosition.x > 20) createPosition.x = 20 - enemyPrefab.transform.localScale.x;

            Instantiate(enemyPrefab, createPosition, Quaternion.Euler(0, 0, 0));
        }
    }
}
