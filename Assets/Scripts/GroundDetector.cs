using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public ParticleSystem dust;
    static Animator anim;
    public AudioSource footstepsCue;

    private void Awake()
    { anim = GetComponentInParent<Animator>(); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            dust.transform.position = new Vector3(transform.position.x, dust.transform.position.y, transform.position.z);

            dust.Play();
            
            if (anim.GetBool("isSprinting") == false)
                footstepsCue.pitch = 0.375f;

            else
                footstepsCue.pitch = 1f;
            

            if (anim.GetBool("isMoving") == true) 
                footstepsCue.Play();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            anim.SetBool("isGrounded", false);
            //footstepsCue.Stop();
        }
    }
}
