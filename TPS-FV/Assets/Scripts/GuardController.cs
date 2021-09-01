using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    private CharacterController characterController;
    private Transform Player;
    private float healPoint;

    private float movingSpd;
    private float groundGrv;
    private float groundDst;
    private bool groundedFlag;
    private Vector3 guardVelocity;
    public Transform groundChk;
    public LayerMask groundMsk;

    public int mapZ;
    public int mapX;

    private float roamDistance;
    private Vector3 initPosition;
    private Vector3 goalPosition;
    private float roamTimer;
    private float roamIntvl;

    private float rushRange;

    public GameObject explodePrefab;

    private enum State
    {
        roam,
        rush,
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

        guardVelocity = Vector3.zero;

        mapZ /= 2;
        mapX /= 2;

        roamDistance = 10f;
        roamTimer = 0f;
        roamIntvl = 2f;

        rushRange = 10f;

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
            if (guardVelocity.y < 0)
            {
                guardVelocity.y = -2f;
            }
        }

        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 10f, transform.position.z);
            return;
        }

        guardVelocity.y += groundGrv * Time.deltaTime;

        characterController.Move(guardVelocity * Time.deltaTime);
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

                case State.rush:
                    RushFunc();
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
            Vector3.Distance(transform.position, Player.position) > rushRange ||
            Physics.Linecast(transform.position, Player.position, groundMsk)
            ? State.roam : State.rush;
    }

    private void RoamFunc()
    {
        transform.LookAt(goalPosition, Vector3.up);

        if (Time.time > roamTimer || Vector3.Distance(transform.position, goalPosition) < roamDistance)
        {
            roamTimer = Time.time + roamIntvl;

            float rx = Random.Range(-1f, 1f);
            float rz = Random.Range(-1f, 1f);

            Vector3 roamPos = initPosition + new Vector3(rx, 0, rz).normalized * Random.Range(10f, 20f);

            if (roamPos.z < -mapZ + 5) roamPos.z = -mapZ + 5;
            if (roamPos.x < -mapX + 5) roamPos.x = -mapX + 5;
            if (roamPos.z > mapZ - 5) roamPos.z = mapZ - 5;
            if (roamPos.x > mapX - 5) roamPos.x = mapX - 5;

            goalPosition = roamPos;
        }

        characterController.Move((goalPosition - transform.position).normalized  * movingSpd * Time.deltaTime);
    }

    private void RushFunc()
    {
        transform.LookAt(Player.position, Vector3.up);

        int playerLvl = Player.GetComponent<PlayerController>().getGameLvl();

        if (playerLvl < 0)
        {

        }
        else if (playerLvl < 10)
        {
            movingSpd = 4;
        }
        else if (playerLvl >= 10 && playerLvl <= 15)
        {
            movingSpd = 5;
        }
        else if (playerLvl > 15)
        {
            movingSpd = 6;
        }

        gameObject.GetComponentInChildren<Sword>().Hold();

        characterController.Move((Player.position - transform.position).normalized * movingSpd * Time.deltaTime);
    }

    private void OverFunc()
    {
        characterState = State.over;

        Player.GetComponent<PlayerController>().AcceptGoalFunc(1);
        GameObject effect = Instantiate(explodePrefab, transform.position, transform.rotation);
        effect.transform.Rotate(-90, 0, 0);
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
