using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Vector3 dir;
    public float time;
    // Update is called once per frame
    void Update()
    {
        if (Time.time>5)
        {
            Destroy(gameObject);
        }
        transform.position += speed * dir * Time.deltaTime;
    }
}
