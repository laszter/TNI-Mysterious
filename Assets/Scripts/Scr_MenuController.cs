using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_MenuController : MonoBehaviour {

    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject selected_bar;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private Text confirmTxt;
    [SerializeField] private Text progressTxt;

    private bool startclicked;
    private bool exitcliked;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startclicked = false;
            exitcliked = false;
            confirmPanel.SetActive(false);
        }
	}

    public void StartButtonClick()
    {
        confirmTxt.text = "Do you want to skip tutorials?";
        startclicked = true;
        confirmPanel.SetActive(true);
    }

    public void LoadTutorial()
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadNewSceneWithProgress("Scn_Tutorials"));
    }

    public void LoadMainScene()
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadNewSceneWithProgress("Scn_Main"));
    }

    public void ExitButtonClcik()
    {
        confirmTxt.text = "Do you want to Exit?";
        exitcliked = true;
        confirmPanel.SetActive(true);
    }

    public void BtnYesOnClick()
    {
        if (startclicked)
        {
            LoadMainScene();
        }
        else if (exitcliked)
        {
            Application.Quit();
        }
    }

    public void BtnNoOnClick()
    {
        if (startclicked)
        {
            LoadTutorial();
        }
        else if (exitcliked)
        {
            exitcliked = false;
            confirmPanel.SetActive(false);
        }
    }

    public void ButtonOnEnterEvent(GameObject btn)
    {
        selected_bar.transform.position = new Vector3(selected_bar.transform.position.x, btn.transform.position.y, selected_bar.transform.position.z);
    }

    private IEnumerator LoadNewSceneWithProgress(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            progressTxt.text = progress * 100f + "";
            yield return null;
        }
    }
}
