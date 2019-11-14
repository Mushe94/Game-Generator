using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
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
                GameObject s = (GameObject)Resources.Load("Prefabs/SSS.prefab");
                GameObject sworld=Instantiate(s, transform.position+Vector3.right*0.65f,transform.rotation);
                sworld.transform.parent = this.transform;
            }
            else
            {
                Destroy(GameObject.Find("SSS"));
            }
            if (data.RangeAttack)
            {
                GameObject s = (GameObject)Resources.Load("Prefabs/Gun.prefab");
                GameObject gun = Instantiate(s, transform.position + Vector3.right *-0.65f, transform.rotation);
                gun.transform.parent = this.transform;
            }
            else
            {
                Destroy(GameObject.Find("Gun"));
            }
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
                Life=0;
                break;

            case 1:
                Debug.Log("No puede morir");
                break;
            case 2:
                Life--;
                break;
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
