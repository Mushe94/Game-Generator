using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Text timeText;
    public Text killsText;

    private float timer;
    public float amountOfTime;
    public int amountofEnemies;
    public Scene[] myscenes;
    public int amountScenes;
    public GameObject enemyOBJ;
    public float instanceTimer;

    public float enemyInstanceEndlessTime;
    public Transform[] instanceEndless;
    private float instanceTimerEndless;

    private bool levelFinished; //CON ESTE BOOL SABES SI YA TERMINO EL NIVEL Y GANO;



    private void Awake()
    {
        
        levelFinished = false;
        if (scriptable.objPlatform == ObjectivePlatformer.BYTIME) // SI EL OBJTIVO ES POR TIEMPO
        {
           
        timer = amountOfTime;

        }
        else
        {
            timer = 0;
        }

        if (scriptable.objPlatform == ObjectivePlatformer.BYKILLING)
        {
            enemies = amountofEnemies;
            killsText.text = enemies + "";
        }

        timeText.text = "" + Mathf.FloorToInt(amountOfTime);
        if (scriptable.pers == Perspective.side) //SI LA PERSPECTIVA ES DE LADO
        {
            offset = new Vector3(0, 0, -10);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = true;
        }
        if (scriptable.pers == Perspective.iso) //SI LA PERSPECTIVA ES ISOMETRICA
        {
            offset = new Vector3(-10, 10, 10);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = false;
        }
        if (scriptable.pers == Perspective.top) //SI LA PERSPECTIVA ES TOP DOWN
        {
            offset = new Vector3(-2, 10, 0);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = true;
        }
        if (scriptable.pers == Perspective.third) // SI LA PERSPECTIVA ES  THIRD PERSON
        {
            offset = new Vector3(0, 0, -5);
            mycam.transform.LookAt(myplayer.transform.position);
            mycam.orthographic = false;
            minY = 0;
            maxY = 50;
        }

       //CHEKEO LAS PERSPECTIVAS PORQUE DEPENDIENDO DE ELLAS, SE VAN A ACOMODAR LAS CAMARAS Y EL MOV DEL PLAYER
       //GONZALO GAY
       //EL PLAYER PUEDE SER REEMPLAZADO POR EL PLAYER DE GONZA

    }
    
    private void LateUpdate() //EN ESTE LATEUPDATE MANEJO LA PERSPECTIVA DE LA CAMARA DEPENDIENDO DEL OFFSET Y LA VISA ORTOGRAFICA O PERSPECTIVA
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
       
    }
    private void Update()
    {
        //MOUSE CONFIG
        currentY -= Input.GetAxis("Mouse Y");
        currentX += Input.GetAxis("Mouse X");
        Mathf.Clamp(currentY, minY, maxY);
        //MOUSE CONFIG

        //SI EL OBJETIVO ES POR TIEMPO, TIMER--, Y SI LLEGA A 0, GANA, SINO VA A SEGUIR SUMANDO TIEMPO
        if(scriptable.objPlatform == ObjectivePlatformer.BYTIME)
        {
            if(!levelFinished) timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timeText.text = "WIN";
                levelFinished = true; //nivel terminado
            }
        }
        else
        {
            if(!levelFinished) timer += Time.deltaTime;
        }
        // SI EL OBJETIVO ES POR TIEMPO, TIMER--, Y SI LLEGA A 0, GANA, SINO VA A SEGUIR SUMANDO TIEMPO


        if(scriptable.objPlatform == ObjectivePlatformer.BYKILLING)
        {
            killsText.text = enemies + "";

            if(enemies <= 0)
            {
                levelFinished = true;//nivel terminado
                timeText.text = "WIN";
            }
        }

        if(scriptable.gm == GameMode.endless && scriptable.pers == Perspective.side)
        {
            Endless();
        }

        //SI EL MODO DE JUEGO ES SURVIVAL VA A INSTANCEAR ENEMIGOS EN RANDOM PLACES DEPENDIENDO DEL AMOUNTOFTIME Y AMOUNTOFENEMIES
        if (scriptable.gm == GameMode.survival)
        {
            var timeToInstance = (amountOfTime / amountofEnemies);
            instanceTimer += Time.deltaTime;
            if(instanceTimer>=timeToInstance)
            {
                var rndm = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                instanceTimer = 0;
                if(scriptable.pers == Perspective.top || scriptable.pers == Perspective.iso|| scriptable.pers == Perspective.third)
                {
                    Instantiate(enemyOBJ, rndm, transform.rotation);
                }
            }
        }
        //SI EL MODO DE JUEGO ES SURVIVAL VA A INSTANCEAR ENEMIGOS EN RANDOM PLACES DEPENDIENDO DEL AMOUNTOFTIME Y AMOUNTOFENEMIES

        timeText.text = timer+ ""; //TIMER TEXT EN CANVAS
    }

    void Endless()
    {
        if(!levelFinished) instanceTimerEndless += Time.deltaTime;
        if(instanceTimerEndless>=enemyInstanceEndlessTime)
        {
            Instantiate(enemyOBJ, instanceEndless[Random.Range(0, 3)].position, transform.rotation);
            instanceTimerEndless = 0;
        }
    }

    IEnumerator LoadmyScene()
    {

            //ESTA ES UN TEST, PERO YA QUE OTRO SE ENCARGA DE LAS CARGAS DE ESCENAS, AL P2
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
