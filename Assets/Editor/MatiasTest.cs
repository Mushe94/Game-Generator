using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[CustomEditor (typeof(MyBehaviour))]
public class MatiasTest : Editor
{
    MyBehaviour _target;
    GUIStyle titleStyle;
    int layerMask = 1 << 9;
    Color mycolor;
    private void OnEnable()
    {
        _target = (MyBehaviour)target;
        titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontStyle = FontStyle.Bold;
        mycolor = Color.red;
        mycolor.a = 0.5f;
    }
    private void OnSceneGUI()
    {
       
        var v = Camera.current.pixelRect;
        GUILayout.Label(_target.name);

        

        RaycastHit hit;
        if (Physics.Raycast(Camera.current.transform.position, Camera.current.transform.forward , out hit, 100, layerMask))
        {
            
        GUILayout.BeginArea(new Rect(Screen.width/2 - 180 ,v.y,200,200));

        GUILayout.Label(hit.collider.gameObject.name,titleStyle);

        GUILayout.EndArea();

           /* var a = hit.collider.gameObject.GetComponent<BoxCollider>();
            var b = hit.collider.gameObject.GetComponent<SphereCollider>();
            if (a != null)
            {

                var col = hit.collider.gameObject.GetComponent<BoxCollider>();
            Handles.color = mycolor;
                Handles.CubeHandleCap(1, hit.collider.transform.position, hit.collider.transform.rotation,
                           col.size.x +2 , EventType.Repaint);

                Debug.Log("aa");
               
            }
            else if (b != null)
            {
                var col = hit.collider.gameObject.GetComponent<SphereCollider>();
                Handles.color = mycolor;
                Handles.SphereHandleCap(1, hit.collider.transform.position, hit.collider.transform.rotation, 
                         col.radius +3, EventType.Repaint);
                Debug.Log("bb");
            }*/
        }
       
        
    }
}
