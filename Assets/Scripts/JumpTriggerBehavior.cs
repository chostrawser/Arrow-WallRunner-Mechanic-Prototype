using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerBehavior : MonoBehaviour
{
    public bool canJumpLeft, canJumpRight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            if (tag == "JumpTrigger_Left")
                canJumpLeft = true;

            else if (tag == "JumpTrigger_Right")
                canJumpRight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Jumpable")
        {
            if (tag == "JumpTrigger_Left")
                canJumpLeft = false;

            else if (tag == "JumpTrigger_Right")
                canJumpRight = false;
        }
    }
}
