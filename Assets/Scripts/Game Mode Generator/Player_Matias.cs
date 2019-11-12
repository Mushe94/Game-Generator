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
            if (manager.scriptable.gm != GameMode.endless)
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
            else
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    transform.position += transform.up * speed * Time.deltaTime;

                }

                if (Input.GetAxis("Vertical") < 0)
                {
                    transform.position += -transform.up * speed * Time.deltaTime;
                }
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
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector3 camF = Camera.main.transform.forward;
            Vector3 camH = Camera.main.transform.right;
            camF.y = 0;
            camH.y = 0;
            camF = camF.normalized;
            camH = camH.normalized;

            Vector3 playermovement = (hor * camH + ver * camF).normalized * speed * Time.deltaTime;

            transform.position += playermovement;

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {  
                transform.forward = playermovement;
            }

            if (Input.GetKeyDown(KeyCode.Space) && manager.scriptable.gm != GameMode.endless)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer ==21)
        {
            manager.points++;
            print("puntos");
        }
        if(collision.gameObject.layer == 22) //la layer 22 es el objetivo en el modo plataforma
        {
            manager.levelFinished = true; //si la toca, gana
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 21)
        {
            manager.points++;
            print("puntos");
        }
    }
}
