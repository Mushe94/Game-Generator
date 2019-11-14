using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    private Player pl;
    private bool MoveOnZ;
    private void Awake()
    {
        var g = GameObject.Find("Manager").GetComponent<GameManager>();
        if (g != null)
        {

            if (g.scriptable.pers != Perspective.side)
            {
                MoveOnZ = true;
                gameObject.AddComponent<Jump>();
            }
          
        }
    }
    private void Start()
    {
        pl = gameObject.GetComponent<Player>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (MoveOnZ)
            {
                transform.position += Vector3.forward * pl.speed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.up * pl.speed * Time.deltaTime;

            }

        }

        if (Input.GetKey(KeyCode.S))
        {
            if (MoveOnZ)
            {
                transform.position += Vector3.back * pl.speed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.down * pl.speed * Time.deltaTime;

            }
        }
    }
}
