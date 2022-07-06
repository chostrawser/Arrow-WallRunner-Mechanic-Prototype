using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(playerController))]
public abstract class playerAbility : MonoBehaviour
{
    protected static playerController playerC;
    protected static Rigidbody rb;
    protected static Animator anim;

    private void Awake()
    {
        playerC = GetComponent<playerController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public abstract void use();

    public virtual void stop()
    { }
}
