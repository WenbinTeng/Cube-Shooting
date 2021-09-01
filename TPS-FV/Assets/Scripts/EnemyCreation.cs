using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreation : MonoBehaviour
{
    public Transform Player;
    public GameObject enemyPrefab;
    private float createTimer;
    private float createIntvl;
    private int createCnt;
    private int createNmb;
    public int mapZ;
    public int mapX;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 0f;
        createIntvl = 30f;
        createCnt = 0;
        createNmb = 30;
        mapZ /= 2;
        mapX /= 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > createTimer)
        {
            createTimer = Time.time + createIntvl;

            createCnt++;

            Vector3 playerPosition = Player.position;

            Vector3 enemyPosition = new Vector3(
                Random.Range(-20f, 20f),
                Random.Range(  5f, 10f),
                Random.Range(-20f, 20f)
                );

            Vector3 createPosition = transform.position + enemyPosition + playerPosition;

            if (createPosition.z < -mapZ + 5) createPosition.z = -mapZ + 5;
            if (createPosition.x < -mapX + 5) createPosition.x = -mapX + 5;
            if (createPosition.z > mapZ - 5) createPosition.z = mapZ - 5;
            if (createPosition.x > mapX - 5) createPosition.x = mapX - 5;

            if (Vector3.Distance(Player.position, createPosition) > 10f)
            {
                Instantiate(enemyPrefab, createPosition, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                createTimer = 0f;
            }
        }

        if (createCnt == 0)
        {
            createTimer = 0f;
        }

        createIntvl = createNmb - Player.GetComponent<PlayerController>().getGameLvl() * 2;

        if (createIntvl < 5)
        {
            createIntvl = 5;
        }
    }
}
