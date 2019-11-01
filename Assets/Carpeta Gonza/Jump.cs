using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Rigidbody rb;
    private Player pl;
    private bool grounded;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
       pl= gameObject.GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&grounded)
        {
            rb.AddForce(Vector3.up * pl.jumpForce);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            grounded =false;
        }
    }
}
