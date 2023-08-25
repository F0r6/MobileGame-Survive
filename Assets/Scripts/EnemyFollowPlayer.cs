using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    //Setting variables
    public float followCheck = 1.0f;
    public float timer = 0f;
    public float meleeRange = 3f;
    public NavMeshAgent enemy;
    public Transform enemyBody; //location of ghost body
    public Transform playerPos;    //location of player 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.Find("Player").transform;

    }

    //Sets the ghost's target location to the player's current location
    public void enemyFollows()
    {
        enemy.SetDestination(playerPos.transform.position);
    }

    void enemyFollowCounter()
    {
        timer = timer - Time.deltaTime;
        if (timer < 0)
        {
            timer = followCheck;
            enemyFollows();
        }
    }

    private bool IsInMeleeRangeOf(Transform target)
    {

        float distance = Vector3.Distance(transform.position, target.position);
        return distance < meleeRange;
    }
}
