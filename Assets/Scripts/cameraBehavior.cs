using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] GameObject lockOn;
    [SerializeField] GameObject player;

    WallRunAbility wallRunner;
    Animator anim;
    Vector3 ogOffset, runOffset, currentOffset, lockOnOGPos, lockOnRunSide;
    float leftX, rightX;
    bool wallRunInvoked = false;

    Quaternion lockOnOGRot;

    private void Awake()
    {
        wallRunner = player.GetComponent<WallRunAbility>();
        anim = player.GetComponent<Animator>();

        ogOffset = cameraOffset;
        runOffset = new Vector3(cameraOffset.x, cameraOffset.y, cameraOffset.z);

        leftX = cameraOffset.x + 5;
        rightX = cameraOffset.x - 5;

        lockOnOGPos = lockOn.transform.localPosition;
        lockOnOGRot = lockOn.transform.localRotation;
    }

    private void OnEnable()
    {
        wallRunner.WallRunEnabled += setOffset;
        wallRunner.WallRunCanceled += resetOffset;
    }

    private void OnDisable()
    {
        wallRunner.WallRunEnabled -= setOffset;
        wallRunner.WallRunCanceled -= resetOffset;
    }

    private void FixedUpdate()
    {
        if (wallRunInvoked == true)    //behavior for when player is running up the wall
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wall Run Front") || anim.GetCurrentAnimatorStateInfo(0).IsName("Wall Run Left"))
            {
                runOffset = new Vector3(leftX, runOffset.y, runOffset.z);
                lockOnRunSide = Vector3.right;
            }
       
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wall Run Right"))
            {
                runOffset = new Vector3(rightX, runOffset.y, runOffset.z);
                lockOnRunSide = Vector3.left;
            }

            lockOn.transform.SetParent(null);
            lockOn.transform.position += new Vector3(wallRunner.wall.GetContact(0).normal.x * .0001f, 0.005f, wallRunner.wall.GetContact(0).normal.z * .0001f);
            lockOn.transform.rotation = Quaternion.FromToRotation(lockOnRunSide, wallRunner.wall.GetContact(0).normal);

            currentOffset = runOffset;
        }

        else   //behavior for when player is running normally
        {
            if (!lockOn.transform.IsChildOf(player.transform))
            {
                lockOn.transform.SetParent(player.transform);
                lockOn.transform.localPosition = lockOnOGPos;
                lockOn.transform.localRotation = lockOnOGRot;
            }

            currentOffset = ogOffset;
        }

        cameraOffset = Vector3.Lerp(cameraOffset, currentOffset, Time.time * 0.005f);

        transform.position = lockOn.transform.TransformPoint(cameraOffset);
        transform.LookAt(lockOn.transform);
    }

    private void setOffset()
    { wallRunInvoked = true; }

    private void resetOffset()
    { wallRunInvoked = false; }
}
