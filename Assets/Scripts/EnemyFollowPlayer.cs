using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed;

    private float dist;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        dist = Vector2.Distance(transform.position, player.transform.position); // Distance between 2 transforms
        Vector2 direction = player.transform.position - transform.position;     

        // Moves the enemy towards target
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }






    ////Setting variables
    //public float followCheck = 1.0f;
    //public float timer = 0f;
    //public float meleeRange = 3f;
    //public NavMeshAgent enemy;
    //public Transform enemyBody; //location of ghost body
    //public Transform playerPos;    //location of player 

    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    playerPos = GameObject.Find("Player").transform;

    //    enemyFollows();

    //}

    ////Sets the ghost's target location to the player's current location
    //public void enemyFollows()
    //{
    //    enemy.SetDestination(playerPos.transform.position);
    //}

    //void enemyFollowCounter()
    //{
    //    timer = timer - Time.deltaTime;
    //    if (timer < 0)
    //    {
    //        timer = followCheck;
    //        enemyFollows();
    //    }
    //}

    //private bool IsInMeleeRangeOf(Transform target)
    //{

    //    float distance = Vector3.Distance(transform.position, target.position);
    //    return distance < meleeRange;
    //}
}
