using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_TutorialsController : MonoBehaviour {

    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpController;
    [SerializeField] private AudioClip switchSound;
    [Header("Step 1")]
    [SerializeField] private Light l1;
    [SerializeField] private GameObject pickObject;
    [SerializeField] private GameObject Chan;
    private float timer;
    private int tutorialStep;
    float distanceMainObj;
    bool timerStop;
    bool start;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private Text progressTxt;

    // Use this for initialization
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        timer = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (!start)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                start = true;
            }
            return;
        }

        if(!timerStop)
        timer += Time.deltaTime;

        if(timer > 5f && tutorialStep == 0)
        {
            Step1Active();
            tutorialStep += 1;
            timerStop = true;
        }else if (tutorialStep == 1)
        {
            distanceMainObj = Vector3.Distance(fpController.transform.position, l1.transform.position);
            if(distanceMainObj > 40f)
            {
                Step1Active();
            }
            if (fpController.GetComponent<Scr_Interactor>().IsCarrying())
            {
                Step2Active();
                tutorialStep += 1;
            }
        }else if(tutorialStep == 2)
        {
            distanceMainObj = Vector3.Distance(fpController.transform.position, l1.transform.position);
            if (distanceMainObj > 40f)
            {
                Step2Active();
            }
            if (Chan.GetComponent<Scr_ChanIdle>().IsHit())
            {
                loadPanel.SetActive(true);
                StartCoroutine(LoadNewSceneWithProgress("Scn_Main"));
                tutorialStep += 1;
            }
        }
    }

    private void Step1Active()
    {
        l1.gameObject.SetActive(true);
        l1.GetComponent<AudioSource>().PlayOneShot(switchSound);
        float yPos = l1.transform.position.y;
        l1.transform.position = fpController.transform.position + (fpController.transform.forward * 20f);
        l1.transform.position = new Vector3(l1.transform.position.x, yPos, l1.transform.position.z);
        pickObject.SetActive(true);
        pickObject.transform.position = new Vector3(l1.transform.position.x, pickObject.transform.position.y, l1.transform.position.z);
    }

    private void Step2Active()
    {
        l1.gameObject.SetActive(true);
        l1.GetComponent<AudioSource>().PlayOneShot(switchSound);
        float yPos = l1.transform.position.y;
        l1.transform.position = fpController.transform.position + (fpController.transform.forward * 20f);
        l1.transform.position = new Vector3(l1.transform.position.x, yPos, l1.transform.position.z);
        Chan.SetActive(true);
        Chan.transform.position = new Vector3(l1.transform.position.x, Chan.transform.position.y, l1.transform.position.z);
        if (!fpController.GetComponent<Scr_Interactor>().IsCarrying())
        {
            Vector3 setPos = fpController.transform.position + (fpController.transform.forward * 15f);
            pickObject.SetActive(true);
            pickObject.transform.position = new Vector3(setPos.x, pickObject.transform.position.y, setPos.z);
        }
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
