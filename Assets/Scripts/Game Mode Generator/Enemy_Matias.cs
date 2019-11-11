using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Matias : MonoBehaviour
{
    Player_Matias player;
    GameManager manager;
    public float speed;
    public float life;
    private void Awake()
    {
        player = FindObjectOfType<Player_Matias>();
        manager = FindObjectOfType<GameManager>();
        //transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (manager.scriptable.objPlatform != ObjectivePlatformer.BYKILLING)
        {
        manager.enemies++;
        }
       
    }
    private void Update()
    {
        if (manager.scriptable.gm != GameMode.endless)
        {
            var dir = player.transform.position - transform.position;
            var newdir = new Vector3(dir.x, transform.position.y, dir.z);
            transform.position += newdir.normalized * speed * Time.deltaTime;
        }
        else
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }

    }
    private void LateUpdate()
    {
        if(life <=0)
        {
            manager.enemies--;
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 20)
        {
            life--;
        }
    }
}
