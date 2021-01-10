using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WallRunAbility : playerAbility
{
    [SerializeField] JumpTriggerBehavior left;
    [SerializeField] JumpTriggerBehavior right;

    public AudioSource steps;
    public AudioSource grunt;

    public event Action WallRunEnabled = delegate { };
    public event Action WallRunCanceled = delegate { };

    public Collision wall;
    bool skipInvoke;                //used to prevent the wall jump method from being triggered more than once from collision

    protected void OnEnable()
    {
        WallRunEnabled += use;
        WallRunCanceled += stop;
    }

    protected void OnDisable()
    {
        WallRunEnabled -= use;
        WallRunCanceled -= stop;
    }

    public void invokeWallRunEnable()   //for invoking events outside this script
    { WallRunEnabled?.Invoke(); }

    public void invokeWallRunCancel()
    { WallRunCanceled?.Invoke(); }

    public override void use()
    {
        float posCorrection;

        playerC.canMove = false;
        playerC.canTurn = false;
        playerC.canShoot = false;

        if (left.canJumpLeft == true && right.canJumpRight == false)            //left run
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.right, wall.GetContact(0).normal);
            posCorrection = 0.3f;

            anim.SetTrigger("wallRunLeft");
        }

        else if (right.canJumpRight == true && left.canJumpLeft == false)       //right run
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.left, wall.GetContact(0).normal);
            posCorrection = 0.3f;

            anim.SetTrigger("wallRunRight");
        }

        else if (right.canJumpRight == true && left.canJumpLeft == true)        //front run
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.back, wall.GetContact(0).normal);
            posCorrection = 2.2f;     //must be significantly large due to the animation

            anim.SetTrigger("wallRunFront");
        }

        else
        {
            WallRunCanceled?.Invoke();
            return;
        }

        grunt.Play();
        transform.Translate((transform.position += wall.GetContact(0).normal * posCorrection) * 0.007f);        //correct position/rotation calculations; front correction must be different from left and right due to their animations
    }

    public override void stop()
    {
        playerC.canMove = true;
        playerC.canTurn = true;
        playerC.canShoot = true;

        skipInvoke = false;
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Jumpable") && anim.GetBool("isSprinting") == true && skipInvoke == false)
        {
            wall = hit;
            WallRunEnabled?.Invoke();

            skipInvoke = true;
            StartCoroutine(StopWallRun());
        }
    }

    IEnumerator StopWallRun()       //fallback in case the skipInvoke isn't reset after wall run 
    {
        yield return new WaitForSeconds(2.5f);

        stop();
    }
}