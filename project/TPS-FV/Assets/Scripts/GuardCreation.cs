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
    public int mapZ;
    public int mapX;

    // Start is called before the first frame update
    void Start()
    {
        createTimer = 0f;
        createIntvl = 5f;
        createCnt = 0;
        createNmb = 5;
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
                5f,
                Random.Range(-20f, 20f)
                );

            Vector3 createPosition = transform.position + enemyPosition + playerPosition;

            if (createPosition.z < -mapZ + 5) createPosition.z = -mapZ + 5;
            if (createPosition.x < -mapX + 5) createPosition.x = -mapX + 5;
            if (createPosition.z > mapZ - 5) createPosition.z = mapZ - 5;
            if (createPosition.x > mapX - 5) createPosition.x = mapX - 5;

            if (Vector3.Distance(Player.position, createPosition) > 10f)
            {
                Instantiate(guardPrefab, createPosition, Quaternion.Euler(0, 0, 0));
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

        createIntvl = createNmb - Player.GetComponent<PlayerController>().getGameLvl() * 0.5f;

        if (createIntvl < 2)
        {
            createIntvl = 2;
        }
    }
}
