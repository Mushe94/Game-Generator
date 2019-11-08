using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public Player_Matias myplayer;
    public GameModeProperties scriptable;
    public Camera mycam;
     Vector3 offset;
    public float smoothSpped;
     float currentY;
     float currentX;

    public int enemies;
   
    float minY, maxY;

    public TextMeshProUGUI timeText;
    private float timer;
    public float amountOfTime;
    public Scene[] myscenes;
    public int amountScenes;
    private void Awake()
    {
        if (scriptable.objEndless == ObjectiveEndless.BYTIME || scriptable.objSurvival == ObjectiveSurvival.BYTIME)
        {
           
        timer = amountOfTime;

        }
        else
        {
            timer = 0;
        }
        timeText.text = "" + amountOfTime;
        if (scriptable.pers == Perspective.side)
        {
            offset = new Vector3(0, 0, -10);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = true;
        }
        if (scriptable.pers == Perspective.iso)
        {
            offset = new Vector3(-10, 10, 10);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = false;
        }
        if (scriptable.pers == Perspective.top)
        {
            offset = new Vector3(-2, 10, 0);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = true;
        }
        if (scriptable.pers == Perspective.third)
        {
            offset = new Vector3(0, 0, -5);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = false;
            minY = 0;
            maxY = 50;
        }

        Enemy_Matias[] enemiesc = FindObjectsOfType<Enemy_Matias>();
        enemies = enemiesc.Length;
        if (scriptable.gm == GameMode.survival)
        {
            timeText.text = amountOfTime + "";
            
        }
    }
    
    private void LateUpdate()
    {
        if(scriptable.pers == Perspective.side || scriptable.pers == Perspective.iso || scriptable.pers == Perspective.top)
        {
            Vector3 targetpos = myplayer.transform.position + offset;
            Vector3 smoothpost = Vector3.Lerp(mycam.transform.position, targetpos, smoothSpped);
            mycam.transform.position = smoothpost;
            mycam.transform.LookAt(myplayer.transform);
        }
        if (scriptable.pers == Perspective.third)
        {
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            mycam.transform.position = myplayer.transform.position + rotation * offset;
            mycam.transform.LookAt(myplayer.transform.position);
        }
        if(timer<=0)
        {

        }
    }
    private void Update()
    {
        currentY -= Input.GetAxis("Mouse Y");
        currentX += Input.GetAxis("Mouse X");
        Mathf.Clamp(currentY, minY, maxY);
        if(scriptable.objEndless == ObjectiveEndless.BYTIME || scriptable.objSurvival == ObjectiveSurvival.BYTIME)
        {
            timer -= Time.deltaTime;

        }else
        {
            timer += Time.deltaTime;
        }

        timeText.text = timer+ "";
    }

    IEnumerator LoadmyScene()
    {
            var current = SceneManager.GetActiveScene();
            for (int i = 0; i < myscenes.Length; i++)
            {
                if(myscenes[i].name == current.name)
                {
                    yield return new WaitForSeconds(2);
                    SceneManager.LoadScene(myscenes[i + 1].name);
                }
            }

    }
    
}
