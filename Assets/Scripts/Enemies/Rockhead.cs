using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockhead : Spikehead
{

    private void Start()
    {
        direction = new Vector3[8];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckForPlayer()
    {
        base.CheckForPlayer();
    }

    protected override void CalculateAttackDirection()
    {
        base.CalculateAttackDirection();
        direction[4] = transform.TransformDirection(1, 1, 0) * sightRange; // right diagnoal up
        direction[5] = transform.TransformDirection(-1, 1, 0) * sightRange; //left diagonal up
        direction[6] = transform.TransformDirection(1, -1, 0) * sightRange; // right diagonal down
        direction[7] = transform.TransformDirection(-1, -1, 0) * sightRange; // left diagonal down
    }

    protected override void StopAttacking()
    {
        base.StopAttacking();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
