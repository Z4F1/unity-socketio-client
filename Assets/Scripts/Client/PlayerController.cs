using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
    public float rotSpeed = 20f;
    public float velocity = 0;
    float rotation = 0;

    Rigidbody rb;
    PlayerUpdate pu;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pu = GameObject.Find("GameManager").GetComponent<PlayerUpdate>();
    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        if (pu.updated)
        {
            rotation = transform.rotation.eulerAngles.y;
            velocity = Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;
            rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.fixedDeltaTime;

            transform.rotation = Quaternion.Euler(0, rotation, 0);
            transform.position += transform.forward * velocity;

            if (!rb.IsSleeping())
            {
                pu.UpdateMyTransform(transform.position, transform.rotation.eulerAngles.y);
            }
        }
    }
}
