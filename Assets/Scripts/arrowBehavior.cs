using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowBehavior : MonoBehaviour
{
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource hitSound;
    public Rigidbody rb;
    Rigidbody targetRB;

    ParticleSystem particle;
    bool hit;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        hit = false;
        rb = GetComponent<Rigidbody>();

        rb.transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnEnable()
    {
        shootSound.Play();
        Destroy(gameObject, 5);
    }

    private void FixedUpdate()
    {
        if (hit == false)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Arrow") && !collision.gameObject.CompareTag("Player") && hit == false)
        {
            hit = true;
            particle.Play();
            hitSound.Play();
            rb.isKinematic = true;

            if (collision.gameObject.CompareTag("Target"))       //if the arrow hits a target
            {
                transform.SetParent(collision.gameObject.GetComponentInChildren<Transform>());
                collision.transform.localScale = new Vector3(-collision.transform.localScale.x, collision.transform.localScale.y, -collision.transform.localScale.z);
                targetRB = collision.gameObject.GetComponent<Rigidbody>();

                targetRB.collisionDetectionMode = CollisionDetectionMode.Continuous;    //conrigures the target's collision detection mode for better results
            } //target actually has to be mirrored, or else the arrow will end up in the wrong position

            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
