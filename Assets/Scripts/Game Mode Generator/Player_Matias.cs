using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Matias : MonoBehaviour
{
    private GameManager manager;
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (manager.scriptable.pers == Perspective.side )
        {

            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.position += transform.right * speed * Time.deltaTime;

            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.position += -transform.right * speed * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if(manager.scriptable.pers == Perspective.iso  || manager.scriptable.pers == Perspective.top)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.position += -transform.forward * speed * Time.deltaTime;

            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.position += -transform.right * speed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if (manager.scriptable.pers == Perspective.third)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                var dir = new Vector3(manager.mycam.transform.right.x, transform.position.y, manager.mycam.transform.right.z);
                transform.position += dir * speed * Time.deltaTime;
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                var dir = new Vector3(manager.mycam.transform.right.x, transform.position.y, manager.mycam.transform.right.z);
                transform.position += -dir * speed * Time.deltaTime;
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                var dir = new Vector3(manager.mycam.transform.forward.x, transform.position.y, manager.mycam.transform.forward.z);
                transform.position +=dir * speed * Time.deltaTime;
                
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                var dir = new Vector3(manager.mycam.transform.forward.x, transform.position.y, manager.mycam.transform.forward.z);
                transform.position += -dir * speed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
