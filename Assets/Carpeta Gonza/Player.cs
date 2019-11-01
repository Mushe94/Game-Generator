﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerScriptable data;

    private Vector3 initialPos;// si vamos a meter checkpoints reemplazar esto 
    public float Life;
    public float speed;
    public float damage;
    public float jumpForce;
    public int deathType;

    private void Awake()
    {
        gameObject.AddComponent<CapsuleCollider>();
        Life = data.Life;
        speed = data.speed;
        damage = data.damage;
        jumpForce = data.jumpForce;
        deathType = data.currentDeath;
    }
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.position;
        if (data.CanJump&&!data.VerticalMovement)
        {
            gameObject.AddComponent<Jump>();
        }
        if (data.VerticalMovement)
        {

        }
    }

    private void Update()
    {
        if (Life <= 0)
        {
            transform.transform.transform.transform.transform.transform.transform.position = initialPos;
        }
    }
    public void Death()//Incluye TakeDamage
    {
        switch (deathType)
        {
            case 0: Debug.Log("se cago muriendo");
                break;

            case 1:Debug.Log("No puede morir");
                break;
            case 2:Debug.Log("Restar vida igual al damage del enemigo");
                break;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer==11)//layer 11=enemigos hasta que decidamos
        {
            Death();
        }
    }
}
