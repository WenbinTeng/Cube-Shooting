using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    private GameObject Player;

    private Rigidbody characterRigidbody;

    private float healPoint;

    private float moveTimer;
    private float moveIntvl;

    public GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        characterRigidbody = gameObject.GetComponent<Rigidbody>();

        healPoint = 100f;

        moveTimer = 0f;
        moveIntvl = 2f;
    }

    // Update is called once per frame
    void Update()
    {

        MoveFunc();

    }

    public void AcceptHurtFunc(int hurt)
    {
        healPoint -= hurt;
    }

    private void MoveFunc()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > moveIntvl)
        {
            moveTimer = 0;

            Vector3 moveOffset = Player.transform.position - transform.position;

            moveOffset.y = moveOffset.y > 0 ? moveOffset.y + 10f : 10f;

            characterRigidbody.AddForce(Random.Range(100f, 300f) * moveOffset.normalized, ForceMode.Force);

            characterRigidbody.MoveRotation(Quaternion.Euler(
                Random.Range(90f, 180f),
                Random.Range(90f, 180f),
                Random.Range(90f, 180f)
            ));
        }

        if (healPoint <= 0)
        {
            OverFunc();
        }
    }

    private void OverFunc()
    {
        Player.GetComponent<PlayerController>().AcceptGoalFunc(1);
        GameObject effct = Instantiate(explodePrefab, transform.position, transform.rotation);
        effct.transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
        Destroy(effct, 2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            healPoint = 0;

            Player.GetComponent<PlayerController>().AcceptHurtFunc(10);
        }
    }
}
