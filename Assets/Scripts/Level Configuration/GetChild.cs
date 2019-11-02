using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetChild : MonoBehaviour
{
    public GameObject spawn;

    public GameObject child;

    private void Update()
    {
        if (transform.childCount == 0)
        {
            GameObject b = Instantiate(spawn);
            b.name = "";
            b.transform.position = transform.position;
            b.transform.parent = transform;
            b.AddComponent<MeshFilter>();
            b.AddComponent<MeshRenderer>();
            child = b;
            b.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
