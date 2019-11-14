using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : MonoBehaviour
{
    public GameObject prefab;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
           var e= Instantiate(prefab, transform.position + transform.forward, transform.rotation);
           var b= e.GetComponent<Bullet>();
            b.dir = transform.forward;
        }
    }
}
