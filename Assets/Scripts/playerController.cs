using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class playerController : MonoBehaviour
{
    static Animator anim;

    public event Action Idle = delegate { };            //events
    public event Action Move = delegate { };
    public event Action <Vector3> Turn = delegate { };
    public event Action Jump = delegate { };
    public event Action <bool> Sprint = delegate { };
    public event Action FireAbility = delegate { };

    public Rigidbody playerRB;
    public ParticleSystem landDust;
    public AudioSource fireCue;                         //variables
    public AudioSource jumpCue;
    public AudioSource landCue;
    public float moveSpeed;
    public float turnSpeed;
    public float jumpStrength;
    public bool canMove = true;
    public bool canTurn = true;
    public bool canShoot = true;

    //instantiators
    void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        playerRB.maxDepenetrationVelocity = 50f;
    }

    void Start()
    { Cursor.lockState = CursorLockMode.Locked; }

    //updater
    private void FixedUpdate()
    {
        checkAbility();
        checkIdle();
        checkMove();
        checkTurn();
        checkJump();
        checkSprint();
    }

    //event code
    private void checkIdle()            
    {
        if (!Input.anyKey)  //is stopped
            Idle?.Invoke();
    }

    private void checkMove()
    {
        if(canMove == false || anim.GetCurrentAnimatorStateInfo(0).IsName("standing draw arrow") || anim.GetCurrentAnimatorStateInfo(0).IsName("standing idle fire"))
            return;

        if (Input.GetKey(KeyCode.W))
            Move?.Invoke();
    }

    private void checkTurn()
    {
        if (canTurn == false)
            return;

        float xInput = Input.GetAxis("Mouse X");
        if (xInput != 0) 
        {
            Vector3 rotation = new Vector3(0, xInput, 0);
            Turn?.Invoke(rotation);
        }
    }

    private void checkJump()
    {
        if (anim.GetBool("isGrounded") == false)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump?.Invoke();
    }

    private void checkSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Sprint?.Invoke(true);
        
        else
            Sprint?.Invoke(false);     
    }

    private void checkAbility()
    {
        if (canShoot == false)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            fireCue?.Play();
            FireAbility?.Invoke();
            canMove = false;
        }
    }

    private void OnEnable()     
    {
        Idle += idle;
        Move += move;
        Jump += jump;
        Turn += turn;
        Sprint += sprint;
    }

    private void OnDisable()
    {
        Idle -= idle;
        Move -= move;
        Jump -= jump;
        Turn -= turn;
        Sprint -= sprint;
    }

    //player's abilities
    private void move()                        
    {
        anim.SetBool("isMoving", true);

        playerRB.MovePosition(transform.localPosition + transform.forward * moveSpeed);

        if (anim.GetBool("isSprinting") == true)    //animations
          anim.SetTrigger("sprintForward");

        else
          anim.SetTrigger("moveForward");

    }

    private void turn(Vector3 turn)
    {
        Quaternion newRotation = Quaternion.Euler(turn * turnSpeed);
        playerRB.MoveRotation(playerRB.rotation * newRotation);
    }

    private void jump()
    {
        jumpCue.Play();

        if (anim.GetBool("isMoving") == true)
            anim.SetTrigger("runJump");

        else
            anim.SetTrigger("standJump");
    }

    private void sprint(bool isSprinting)
    {
        if(isSprinting == true)
        {
            moveSpeed = .5f;
            anim.SetBool("isSprinting", true);
        }

        else 
        {
            moveSpeed = .1f;
            anim.SetBool("isSprinting", false);
        }
    }

    private void idle()
    {
        anim.SetTrigger("stop");
        anim.SetBool("isMoving", false);
        anim.SetBool("isSprinting", false);
    }

}