using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public PlayerScriptable data;

    private Vector3 initialPos;// si vamos a meter checkpoints reemplazar esto 
    public float Life;
    private float initialLife;
    public float speed;
    public float damage;
    public float jumpForce;
    public int deathType;
    private MeshRenderer plMat;
    private void OnEnable()
    {
        if (data != null)
        {

            plMat = GetComponent<MeshRenderer>();
            Life = data.Life;
            speed = data.speed;
            damage = data.damage;
            jumpForce = data.jumpForce;
            deathType = data.currentDeath;


            foreach (Material material in Resources.LoadAll<Material>("Prefabs/"))
            {
                if (material.name == "PlayerMat")
                {
                    plMat.material = material;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddComponents();

        initialLife = Life;
        initialPos = transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.position;
    }

    private void Update()
    {
        if (Life <= 0)
        {
            transform.transform.transform.transform.transform.transform.transform.position = initialPos;
            Life = initialLife;
        }
    }
    public void Death()//Incluye TakeDamage
    {
        switch (deathType)
        {
            case 0:
                Life = 0;
                break;

            case 1:
                Debug.Log("No puede morir");
                break;
            case 2:
                Life--;
                break;
        }
    }

    private void AddComponents()
    {

        if (data.HorizontalMovement)
        {
            gameObject.AddComponent<HorizontalMovement>();
        }
        else
        {
            Destroy(GetComponent<HorizontalMovement>());
        }
        if (data.CanJump && !data.VerticalMovement)
        {
            gameObject.AddComponent<Jump>();
        }
        else
        {
            Destroy(GetComponent<Jump>());
        }
        if (data.VerticalMovement)
        {
            gameObject.AddComponent<VerticalMovement>();
        }
        else
        {
            Destroy(GetComponent<VerticalMovement>());
        }

        if (data.MeleAttack)
        {
            foreach (GameObject material in Resources.LoadAll<GameObject>("Prefabs/"))
            {
                if (material.name == "SSS")
                {
                    GameObject s = material;
                    GameObject sworld = Instantiate(s, transform.position , transform.rotation);
                    sworld.transform.forward = transform.forward;
                    sworld.transform.parent = this.transform;
                }
            }
         
        }
        else
        {
            Destroy(GameObject.Find("SSS"));
        }
        if (data.RangeAttack)
        {
            foreach (GameObject material in Resources.LoadAll<GameObject>("Prefabs/"))
            {
                if (material.name == "Gun")
                {
                    GameObject o = material;
                    GameObject gun = Instantiate(o, transform.position + Vector3.right * -0.65f, transform.rotation);
                    gun.transform.forward = transform.forward;
                    gun.transform.parent = this.transform;
                }
            }
           
        }
        else
        {
            Destroy(GameObject.Find("Gun"));
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Death();
        }
    }
}
