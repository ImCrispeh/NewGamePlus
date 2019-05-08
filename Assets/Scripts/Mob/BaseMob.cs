using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseMob : NetworkBehaviour {

    public int sightRange;
    public int attackRange;

    public int baseSpeed;
    private int speed;
    private GameObject target;

    private Vector3 newPosition;
    private Vector3 spawnPosition;

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        speed = baseSpeed;
        spawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        ChooseTarget();
        Move();
        Attack();
	}

    private void ChooseTarget()
    {
        float distance = sightRange;
        target = null;
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("player"))
        {
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(player.transform.position, transform.position);
                target = player;
            }
        }
    }

    private void Move()
    {
        if (target == null)
        {
            newPosition = spawnPosition;
        }
        else if(Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            newPosition = target.transform.position;
        }
        else
        {
            newPosition = transform.position;
        }
        RpcMove(newPosition, speed);
    }

    [ClientRpc]
    void RpcMove(Vector3 position,float speed)
    {
        if (position == transform.position)
        {
            animator.SetFloat("animationstate", 0);
            return;
        }
        animator.SetFloat("animationstate", 1);
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        transform.LookAt(position,Vector3.zero);
    }

    private void Attack()
    {

    }
}
