using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    private GameObject Player;

    private float healPoint;

    private float movingSpd;
    private float groundGrv;
    private float groundDst;
    private bool groundedFlag;
    private Vector3 guardVelocity;
    public Transform groundChk;
    public LayerMask groundMsk;

    private float rushDistance;
    private float roamDistance;
    private Vector3 initPosition;
    private Vector3 goalPosition;
    private float roamTimer;
    private float roamIntvl;
    private bool rushFlag;

    private Color colorRigister;

    private enum State
    {
        roam,
        rush,
        over,
    }
    private State characterState;

    public GameObject explodePrefab;

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

        rushDistance = 10f;
        roamDistance = 5f;
        roamTimer = 0f;
        roamIntvl = 2f;

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
            if (guardVelocity.y < 0)
            {
                guardVelocity.y = -2f;
            }
        }

        if (transform.position.y < 0)
        {
            gameObject.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        guardVelocity.y += groundGrv * Time.deltaTime;

        transform.Translate(guardVelocity * Time.deltaTime, Space.World);
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
        characterState =
            Vector3.Distance(transform.position, Player.transform.position) > rushDistance ||
            Physics.Linecast(transform.position, Player.transform.position, 1 << LayerMask.NameToLayer("Wall"))
            ? State.roam : State.rush;


        if (!rushFlag)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color =
                Vector3.Distance(transform.position, Player.transform.position) > rushDistance ||
                Physics.Linecast(transform.position, Player.transform.position, 1 << LayerMask.NameToLayer("Wall"))
                ? colorRigister : Color.white;
        }
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(gameObject.GetComponentInChildren<Renderer>().material.color, colorRigister, Time.deltaTime * 2);
        }
    }

    private void RoamFunc()
    {
        rushFlag = false;

        movingSpd = 2f;

        transform.LookAt(goalPosition, Vector3.up);

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
        rushFlag = true;

        movingSpd = movingSpd + Time.deltaTime / 2f;

        movingSpd = movingSpd > 5f ? movingSpd : 5f;

        transform.LookAt(new Vector3(Player.transform.position.x, 0.5f, Player.transform.position.z), Vector3.up);

        transform.Translate((Player.transform.position - transform.position).normalized * movingSpd * Time.deltaTime, Space.World);
    }

    private void OverFunc()
    {
        characterState = State.over;

        Player.GetComponent<PlayerController>().AcceptGoalFunc(1);
        GameObject effct = Instantiate(explodePrefab, transform.position, transform.rotation);
        effct.transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
        Destroy(effct, 2f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.GetComponent<PlayerController>().AccepthurtFunc(20);

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
