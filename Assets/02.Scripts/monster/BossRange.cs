using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRange : EnemyAttack
{
    public Transform target;
    public Rigidbody ballrigid;
    public float turn;
    public float ballVelocity;


    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void FixedUpdate() // À¯µµÅº
    {

        ballrigid.velocity = transform.forward * ballVelocity;
        var ballTargetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
        ballrigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));
    }

}
