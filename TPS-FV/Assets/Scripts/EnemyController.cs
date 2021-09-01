using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterController characterController;
    private Transform Player;
    private float healPoint;

    private float movingSpd;
    private float groundGrv;
    private float groundDst;
    private bool groundedFlag;
    private Vector3 enemyVelocity;
    public Transform groundChk;
    public LayerMask groundMsk;

    public int mapZ;
    public int mapX;

    private float roamDistance;
    private Vector3 initPosition;
    private Vector3 goalPosition;
    private float roamTimer;
    private float roamIntvl;

    private float fireRange;
    private float fireTimer;
    private float fireIntvl;

    public GameObject explodePrefab;

    private enum State
    {
        roam,
        fire,
        over,
    }
    private State characterState;


    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        Player = GameObject.Find("Player").transform;
        healPoint = 100f;

        movingSpd = 3.0f;
        groundGrv = -20f;
        groundDst = 0.6f;
        groundedFlag = false;
        initPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        goalPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);

        enemyVelocity = Vector3.zero;

        mapZ /= 2;
        mapX /= 2;

        roamDistance = 10f;
        roamTimer = 0f;
        roamIntvl = 2f;

        fireRange = 15f;
        fireTimer = 0.0f;
        fireIntvl = 1.0f;

        characterState = State.roam;
    }

    // Update is called once per frame
    void Update()
    {

        FallFunc();

        MoveFunc();

    }

    public void AcceptHurtFunc(int hurt)
    {
        healPoint -= hurt;
    }

    private void FallFunc()
    {
        groundedFlag = Physics.CheckBox(groundChk.position, groundDst * Vector3.one, Quaternion.identity, groundMsk);

        if (groundedFlag)
        {
            if (enemyVelocity.y < 0f)
            {
                enemyVelocity.y = -2f;
            }
        }

        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 10f, transform.position.z);
            return;
        }

        enemyVelocity.y += groundGrv * Time.deltaTime;

        characterController.Move(enemyVelocity * Time.deltaTime);
    }

    private void MoveFunc()
    {
        if (groundedFlag)
        {
            switch (characterState)
            {
                case State.roam:
                    RoamFunc();
                    FindFunc();
                    break;

                case State.fire:
                    FireFunc();
                    FindFunc();
                    break;

                case State.over:
                    break;
            }
        }

        if (healPoint <= 0)
        {
            OverFunc();
        }
    }

    private void FindFunc()
    {
        characterState =
            Vector3.Distance(transform.position, Player.position) > fireRange ||
            Physics.Linecast(transform.position, Player.position, groundMsk)
            ? State.roam : State.fire;
    }

    private void RoamFunc()
    {
        transform.LookAt(goalPosition, Vector3.up);

        if (Time.time > roamTimer || Vector3.Distance(transform.position, goalPosition) < roamDistance)
        {
            roamTimer = Time.time + roamIntvl;

            float rx = Random.Range(-1f, 1f);
            float rz = Random.Range(-1f, 1f);

            Vector3 roamPosition = initPosition + new Vector3(rx, 0, rz).normalized * Random.Range(10f, 20f);

            if (roamPosition.z < -mapZ + 5) roamPosition.z = -mapZ + 5;
            if (roamPosition.x < -mapX + 5) roamPosition.x = -mapX + 5;
            if (roamPosition.z > mapZ - 5) roamPosition.z = mapZ - 5;
            if (roamPosition.x > mapX - 5) roamPosition.x = mapX - 5;

            goalPosition = roamPosition;
        }

        characterController.Move((goalPosition - transform.position).normalized * movingSpd * Time.deltaTime);
    }

    private void FireFunc()
    {
        transform.LookAt(Player.position);

        if (Time.time > fireTimer)
        {
            fireTimer = Time.time + fireIntvl;

            int playerLvl = Player.GetComponent<PlayerController>().getGameLvl();

            if (playerLvl <= 5)
            {
                fireIntvl = 1.0f;
            }
            else if (playerLvl > 5 && playerLvl <= 10)
            {
                fireIntvl = 1.5f;
            }
            else if (playerLvl > 10 && playerLvl <= 15)
            {
                fireIntvl = 1.0f;
            }
            else if (playerLvl > 15)
            {
                fireIntvl = 1.5f;
            }

            int bulletNum = playerLvl <= 10 ? 1 : 3;

            for (int i = 0; i < bulletNum; i++)
            {
                gameObject.GetComponentInChildren<Laser>().Fire();
            }  
        }
    }

    private void OverFunc()
    {
        characterState = State.over;

        Player.GetComponent<PlayerController>().AcceptGoalFunc(5);
        GameObject effect = Instantiate(explodePrefab, transform.position, transform.rotation);
        effect.transform.Rotate(-90f, 0, 0);
        Destroy(effect, 3f);
        Destroy(gameObject);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            OverFunc();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            roamTimer = 0;
        }
    }
}