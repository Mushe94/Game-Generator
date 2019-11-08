using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    private Player pl;
    private void Start()
    {
        pl = gameObject.GetComponent<Player>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up*pl.speed;
            
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down*pl.speed;
        }
    }
}
