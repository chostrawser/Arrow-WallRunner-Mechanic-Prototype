using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class arrowAbility : playerAbility
{ 
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject arrowSpawner;
    [SerializeField] float fireForce;

    public event Action Fire = delegate { };

    GameObject arrow;
    Rigidbody arrowRB;

    private void LateUpdate()
    {
        checkIfFired();    
    }

    public override void use()          //use() and spawnArrow() are broken up to sync the animations
    {
        anim.SetTrigger("fireArrow");
    }

    private void OnEnable()
    {
        playerC.FireAbility += use;
        Fire += spawnArrow;
    }

    private void OnDisable()
    {
        playerC.FireAbility -= use;
        Fire -= spawnArrow;
    }

    private void checkIfFired()
    {
        if (anim.GetBool("isArrowFired") == true)
            Fire?.Invoke();   
    }
  
    private void spawnArrow()
    {
        arrow = Instantiate(arrowPrefab, arrowSpawner.transform.position, Quaternion.identity);
        arrowRB = arrow.GetComponent<Rigidbody>();
        arrowRB.velocity = transform.forward * fireForce;

        anim.SetBool("isArrowFired", false);
        playerC.canMove = true;
    }
}
