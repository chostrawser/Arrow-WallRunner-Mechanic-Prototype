using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public ParticleSystem dust;
    static Animator anim;

    private void Awake()
    { anim = GetComponentInParent<Animator>(); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            dust.transform.position = new Vector3(transform.position.x, dust.transform.position.y, transform.position.z);

            if (dust.isPlaying)
                dust.Stop();

            dust.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            anim.SetBool("isGrounded", false);       
    }
}
