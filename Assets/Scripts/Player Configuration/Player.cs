using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Player : MonoBehaviour
{
    public PlayerScriptable data;

    private Vector3 initialPos;// si vamos a meter checkpoints reemplazar esto 
    public float Life;
    public float speed;
    public float damage;
    public float jumpForce;
    public int deathType;
    public MeshRenderer plMat;
    private void OnEnable()
    {
        if (data != null)
        {

            plMat = gameObject.GetComponent<MeshRenderer>();
            Life = data.Life;
            speed = data.speed;
            damage = data.damage;
            jumpForce = data.jumpForce;
            deathType = data.currentDeath;
            if (data.HorizontalMovement)
            {
                gameObject.AddComponent<HorizontalMovement>();
            }
            if (data.CanJump && !data.VerticalMovement)
            {
                gameObject.AddComponent<Jump>();
            }
            if (data.VerticalMovement)
            {
                gameObject.AddComponent<VerticalMovement>();
            }
            if (data.MeleAttack)
            {
                gameObject.AddComponent<MeleAttack>();

            }
        }
      
    }

    // Start is called before the first frame update
    void Start()
    {
        plMat.material = Resources.Load<Material>("/Prefabs/PlayerMat.mat");
        initialPos = transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.position;
       
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
            case 0:
                Debug.Log("se cago muriendo");
                break;

            case 1:
                Debug.Log("No puede morir");
                break;
            case 2:
                Debug.Log("Restar vida igual al damage del enemigo");
                break;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 11)//layer 11=enemigos hasta que decidamos
        {
            Death();
        }
    }
}
