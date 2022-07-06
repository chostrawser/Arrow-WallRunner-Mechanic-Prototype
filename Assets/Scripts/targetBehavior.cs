using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetBehavior : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;

    public float hSpeed;
    public float vSpeed;
    public float altitude;
    public Vector3 tempPos;

    bool isHit;
    AudioSource audio;
    MeshRenderer mesh;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        mesh = GetComponentInChildren<MeshRenderer>();

        player = FindObjectOfType<playerController>().gameObject;
    }

    private void Start()
    {
        tempPos = transform.position;
        isHit = false; 
    }

    private void FixedUpdate()
    {
        if (isHit == false)     //hover effect
        {
            rb.transform.LookAt(player.transform);
         
            tempPos.x += hSpeed;
            tempPos.y += Mathf.Sin(Time.realtimeSinceStartup * vSpeed) * altitude;
            transform.position = tempPos;
        }
    }

    private void OnCollisionEnter(Collision collision)  //what happens when hit
    {
        if (collision.gameObject.CompareTag("Arrow") && isHit == false)
        {
            isHit = true;

            audio.Play();
            mesh.material.SetColor("_Color", Color.gray);

            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = -transform.forward * 5;
            rb.rotation = Quaternion.LookRotation(rb.velocity);

            Destroy(gameObject, 3);
        }
    }
}
