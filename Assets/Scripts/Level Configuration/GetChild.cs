using UnityEngine;

[ExecuteInEditMode]
public class GetChild : MonoBehaviour
{
    public GameObject spawn;
    [HideInInspector]
    public GameObject child;

    private void Update()
    {
        if (transform.childCount == 0)
        {
            GameObject b = Instantiate(spawn);
            b.name = "";
            b.transform.position = transform.position;
            b.transform.parent = transform;
            if (!b.GetComponent<MeshFilter>())
            {
                b.AddComponent<MeshFilter>();
            }
            if (!b.GetComponent<MeshRenderer>())
            {
                b.AddComponent<MeshRenderer>();
            }
            child = b;
            b.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
