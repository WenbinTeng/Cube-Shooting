using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject Player;

    private float healPoint;

    private float movingSpd;
    private float groundGrv;
    private float groundDst;
    private bool groundedFlag;
    private Vector3 enemyVelocity;
    public Transform groundChk;
    public LayerMask groundMsk;

    private float rushDistance;
    private float roamDistance;
    private Vector3 initPosition;
    private Vector3 goalPosition;
    private float roamTimer;
    private float roamIntvl;
    private float rushTimer;
    private float rushIntvl;
    private float coolTimer;
    private float coolIntvl;
    private bool crossFlag;

    private Color colorRigister;

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
        Player = GameObject.Find("Player");

        healPoint = 100f;

        movingSpd = 2.0f;
        groundGrv = -20f;
        groundDst = 0.5f;
        groundedFlag = false;
        initPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        goalPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);

        enemyVelocity = Vector3.zero;

        rushDistance = 10f;
        roamDistance = 5f;
        roamTimer = 0f;
        roamIntvl = 2f;

        crossFlag = true;

        rushTimer = 0f;
        rushIntvl = 2f;
        coolTimer = 0f;
        coolIntvl = 2f;

        colorRigister = gameObject.GetComponentInChildren<Renderer>().material.color;
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
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        enemyVelocity.y += groundGrv * Time.deltaTime;

        transform.Translate(enemyVelocity * Time.deltaTime, Space.World);
    }

    private void MoveFunc()
    {
        if (!groundedFlag)
        {
            return;
        }

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

        if (healPoint <= 0)
        {
            OverFunc();
        }
    }

    private void FindFunc()
    {
        characterState = Vector3.Distance(transform.position, Player.transform.position) < rushDistance ? State.rush : State.roam;
    }

    private void RoamFunc()
    {
        transform.LookAt(goalPosition, Vector3.up);

        gameObject.GetComponentInChildren<Renderer>().material.color = colorRigister;

        if (Time.time > roamTimer || Vector3.Distance(transform.position, goalPosition) < roamDistance)
        {
            roamTimer = Time.time + roamIntvl;

            float rx = Random.Range(-1f, 1f);
            float rz = Random.Range(-1f, 1f);

            Vector3 roamPosition = initPosition + new Vector3(rx, 0, rz).normalized * Random.Range(5f, 20f);

            if (roamPosition.z < -20) roamPosition.z = -20 + transform.localScale.z;
            if (roamPosition.x < -20) roamPosition.x = -20 + transform.localScale.x;
            if (roamPosition.z > 20) roamPosition.z = 20 - transform.localScale.z;
            if (roamPosition.x > 20) roamPosition.x = 20 - transform.localScale.x;

            goalPosition = roamPosition;
        }

        transform.Translate((goalPosition - transform.position).normalized * movingSpd * Time.deltaTime, Space.World);
    }

    private void RushFunc()
    {
        transform.LookAt(Player.transform.position);

        initPosition = transform.position;
        goalPosition = transform.position;

        Vector3 offset = Player.transform.position - transform.position;

        if (Physics.Linecast(transform.position, Player.transform.position, 1 << LayerMask.NameToLayer("Wall")))
        {
            if (crossFlag)
            {
                crossFlag = false;

                rushTimer = Time.time + rushIntvl * 0.5f;
            }

            if (Time.time > rushTimer)
            {
                rushTimer = Time.time + rushIntvl;
                coolTimer = Time.time + coolIntvl;

                transform.position += offset * 0.5f;

                gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            crossFlag = true;

            if (Time.time > coolTimer)
            {
                offset.y = 0.5f;
                transform.Translate(offset.normalized * movingSpd * Time.deltaTime * 1.5f, Space.World);
                transform.LookAt(new Vector3(Player.transform.position.x, 0.5f, Player.transform.position.z), Vector3.up);
            }
            else
            {
                offset.y = 0.5f;
                transform.Translate(offset.normalized * movingSpd * Time.deltaTime * 0.5f, Space.World);
                transform.LookAt(new Vector3(Player.transform.position.x, 0.5f, Player.transform.position.z), Vector3.up);
            }

            gameObject.GetComponentInChildren<Renderer>().material.color = Color.Lerp(gameObject.GetComponentInChildren<Renderer>().material.color, colorRigister, Time.deltaTime * 1);
        }
    }

    private void OverFunc()
    {
        characterState = State.over;

        Player.GetComponent<PlayerController>().AcceptGoalFunc(5);
        GameObject effct = Instantiate(explodePrefab, transform.position, transform.rotation);
        effct.transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
        Destroy(effct, 2f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.GetComponent<PlayerController>().AccepthurtFunc(50);

            OverFunc();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            roamTimer = 0;
        }
    }
}