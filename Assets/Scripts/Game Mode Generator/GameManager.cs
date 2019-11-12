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
    
    public GameObject objCoins;
    public GameObject objPlatform;
     Vector3 offset;
    public float smoothSpped;
     float currentY;
     float currentX;

    public int enemies;
   
    float minY, maxY;

    public Text timeText;
    public Text killsText;
    public Text pointsText;

    public int points; //IMPORTANTE, TIENE QUE CHOCAR CON LAYER 21 PARA SUMAR PUNTOS
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

    public bool levelFinished; //CON ESTE BOOL SABES SI YA TERMINO EL NIVEL Y GANO;



    private void Awake()
    {
        
        levelFinished = false;
        if (scriptable.objPlatform == ObjectivePlatformer.BYTIME && scriptable.gm != GameMode.endless) // SI EL OBJTIVO ES POR TIEMPO
        {
           
        timer = amountOfTime;

        }
        else
        {
            timer = 0;
        }

        if(scriptable.objPlatform == ObjectivePlatformer.CollectCoins)
        {
            objPlatform.SetActive(false);
        }
        else if(scriptable.objPlatform == ObjectivePlatformer.GetToPointB)
        {
            objCoins.SetActive(false);
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
        if(scriptable.objPlatform == ObjectivePlatformer.BYTIME && scriptable.gm != GameMode.endless)
        {
            if(!levelFinished) timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timeText.text = "WIN";
                levelFinished = true; //nivel terminado
            }
            timeText.text = Mathf.FloorToInt(timer) + "";
        }
        else if (!levelFinished && scriptable.objPlatform != ObjectivePlatformer.BYPOINTS)
        {
            timer += Time.deltaTime;
            timeText.text = Mathf.FloorToInt(timer) + ""; //TIMER TEXT EN CANVAS
        
       
        }
        // SI EL OBJETIVO ES POR TIEMPO, TIMER--, Y SI LLEGA A 0, GANA, SINO VA A SEGUIR SUMANDO TIEMPO

        //si el objetivo es matar
        if(scriptable.objPlatform == ObjectivePlatformer.BYKILLING)
        {
            killsText.text = enemies + "";

            if(enemies <= 0)
            {
                levelFinished = true;//nivel terminado
                timeText.text = "WIN";
            }
        }
        //si el objetivo es matar

        //si es endless
        if (scriptable.gm == GameMode.endless )
        {
            Endless();
        }
        //si es endless

        //SI EL MODO DE JUEGO ES SURVIVAL VA A INSTANCEAR ENEMIGOS EN RANDOM PLACES DEPENDIENDO DEL AMOUNTOFTIME Y AMOUNTOFENEMIES
        if (scriptable.gm == GameMode.survival)
        {
            var timeToInstance = (amountOfTime / amountofEnemies);
            instanceTimer += Time.deltaTime;
            if(instanceTimer>=timeToInstance)
            {
                if (scriptable.pers == Perspective.top || scriptable.pers == Perspective.iso || scriptable.pers == Perspective.third)
                {
                    var rndm= new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                    Instantiate(enemyOBJ, rndm, transform.rotation);
                }
                else
                {
                    var rndm = new Vector3(Random.Range(-10, 30), 0, 0);
                    Instantiate(enemyOBJ, rndm, transform.rotation);
                }
                

                instanceTimer = 0;
               
                
            }
        }
        //SI EL MODO DE JUEGO ES SURVIVAL VA A INSTANCEAR ENEMIGOS EN RANDOM PLACES DEPENDIENDO DEL AMOUNTOFTIME Y AMOUNTOFENEMIES

        
    }

    void Endless()
    {
        if(!levelFinished) instanceTimerEndless += Time.deltaTime;
        if(instanceTimerEndless>=enemyInstanceEndlessTime)
        {
            Instantiate(enemyOBJ, instanceEndless[Random.Range(0, instanceEndless.Length)].position, transform.rotation);
            instanceTimerEndless = 0;
        }

        if(scriptable.objPlatform == ObjectivePlatformer.BYPOINTS)
        {
            timeText.text = points + "";
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
