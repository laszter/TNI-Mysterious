using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scr_GameController : MonoBehaviour {
    private static Scr_GameController mInstance;
    public static Scr_GameController Instance
    {
        get
        {
            return mInstance;
        }
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    private bool gameOver;
    public bool GameOver
    {
        get
        {
            return gameOver;
        }
        set
        {
            gameOver = value;

        }
    }
    private int gameOverType;
    public int GameOverType
    {
        get
        {
            return gameOverType;
        }
        set
        {
            gameOverType = value;

        }
    }
    private bool gameOverTriggered;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ajchan;
    [SerializeField] private GameObject ajchanHead;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private Image bg;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Image fearBar;

    private float fearlevel;
    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;

    public bool pause = false;

    bool goCamLerp;
    float goTime = 5f;
    float goTimer;

    bool getScore;

    // Use this for initialization
    void Start () {
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gameOverType = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOver && !gameOverTriggered)
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            player.GetComponent<Scr_Interactor>().DropObject();
            ajchan.GetComponent<Animator>().speed = 1f;
            ajchan.GetComponent<Animator>().SetBool("walk", false);
            ajchan.GetComponent<Animator>().enabled = false;
            fearlevel = ajchan.GetComponent<Scr_AjChan>().scaryLevel;
            gameOverTriggered = true;

            switch (gameOverType)
            {
                case 0:
                    goCamLerp = true;
                    break;
                case 1:
                    gameOverUI.SetActive(true);
                    page2.SetActive(true);
                    gameOverText.text = "\"Professor has gone\"";
                    Cursor.lockState = CursorLockMode.None;
                    gameOverUI.GetComponent<Scr_UIController>().DefaultUI();
                    getScore = true;
                    break;
            }
        }
        if (goCamLerp)
        {
            goTimer += Time.deltaTime;
            if (goTimer >= goTime)
            {
                goTimer = goTime;
                gameOverUI.SetActive(true);
                page1.SetActive(true);
                gameOverText.text = "\"Professor has found you\"";
                Cursor.lockState = CursorLockMode.None;
                gameOverUI.GetComponent<Scr_UIController>().DefaultUI();
                Time.timeScale = 0f;
                goCamLerp = false;
            }
            
            playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, ajchanHead.transform.position + (ajchanHead.transform.forward * 0.6f) - new Vector3(0,0.2f,0), goTimer / goTime);
            LookAtTarget(playerCam, ajchanHead, 250f);
        }
        if (getScore)
        {
            goTimer += Time.deltaTime;
            if (goTimer >= goTime)
            {
                goTimer = goTime;
                getScore = false;
            }
            fearBar.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, fearBar.rectTransform.sizeDelta.y), new Vector2(fearlevel, fearBar.rectTransform.sizeDelta.y), goTimer / goTime);
            if(fearlevel < 30)
            {
                fearBar.color = Vector4.Lerp(Color.white,Color.green,goTimer/goTime);
            }else if (fearlevel < 70)
            {
                fearBar.color = Vector4.Lerp(Color.white, Color.yellow, goTimer / goTime);
            }
            else
            {
                fearBar.color = Vector4.Lerp(Color.white, Color.red, goTimer / goTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (!pause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
	}

    protected void LookAtTarget(GameObject originObj,GameObject targetLookAt,float rotSpd)
    {
        if (targetLookAt == null) return;

        Vector3 targetPos = targetLookAt.transform.position;
        targetPos = new Vector3(targetPos.x, targetPos.y-0.2f, targetPos.z);

        Vector3 desiredDirection = targetPos - originObj.transform.position;
        Vector3 rot = Vector3.RotateTowards(originObj.transform.forward, desiredDirection, Mathf.Deg2Rad * rotSpd * Time.deltaTime, 1);
        originObj.transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pause = true;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        pauseUI.SetActive(true);
        pauseUI.GetComponent<Scr_UIController>().DefaultUI();
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Scn_Menu");
    }
}
