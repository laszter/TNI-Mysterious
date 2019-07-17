using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Light : MonoBehaviour {

    private GameObject player;
    private MeshRenderer mesh;
    private Light neonLight;
    [SerializeField] private Material turnOn;
    [SerializeField] private Material turnOff;
    [SerializeField] private Material turnOnDim;
    [SerializeField] public float lightFlashing = 0;
    private float delayLightFlashing;
    private bool lightActive;
    private bool lightControlActive;
    private bool tempLightEnable;
    private float tempIntensity;
    private float fadeIntensity = 0.3f;
    private bool thisLightOnEvent;
    public float yaxisDistanceFromPlayer;

    public bool inView;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        neonLight = GetComponent<Light>();
        if(neonLight == null)
            neonLight = GetComponentInChildren<Light>();
        
        mesh = GetComponent<MeshRenderer>();

        thisLightOnEvent = false;
        tempLightEnable = neonLight.enabled;
        lightControlActive = true;
        tempIntensity = neonLight.intensity;
    }

    // Update is called once per frame
    void Update() {
        if (lightControlActive)
        {
            if (lightActive||player == null)
                LightFlashControl();

            if(player != null)
            LightManage();
        }
        else
        {
            if (mesh != null) mesh.material = turnOff;
            neonLight.enabled = false;
        }
    }

    private void LightManage()
    {
        yaxisDistanceFromPlayer = Vector3.Distance(new Vector3(0, transform.position.y), new Vector3(0, player.transform.position.y));
        if(transform.position.y - player.transform.position.y > -1.5f && transform.position.y - player.transform.position.y < 8.6f)
        {
            lightActive = true;
        }
        else if (thisLightOnEvent)
        {
            lightActive = true;
        }
        else
        {
            lightActive = false;
            if(mesh != null) mesh.material = turnOff;
            neonLight.enabled = false;
        }
    }

    private void LightFlashControl()
    {
        if (lightFlashing == 0) {
            if(tempLightEnable)
                if (mesh != null) mesh.material = turnOn;
            else
                if (mesh != null) mesh.material = turnOff;
            neonLight.enabled = tempLightEnable;
            neonLight.intensity = tempIntensity;
            return;
        }

        if (delayLightFlashing <= 0) {
            if (!neonLight.enabled || neonLight.intensity != tempIntensity)
            {
                if (mesh != null) mesh.material = turnOn;
                neonLight.enabled = true;
                neonLight.intensity = tempIntensity;
                delayLightFlashing = Random.Range(0, lightFlashing);
            }
            else
            {
                if (mesh != null) mesh.material = turnOnDim;
                //neonLight.enabled = false;
                neonLight.intensity = fadeIntensity;
                delayLightFlashing = Random.Range(0, 0.5f);
            }
        }
        else delayLightFlashing -= 1 * Time.deltaTime;
    }

    public bool IsLightActive()
    {
        return lightActive;
    }

    public void SetLightControlActive(bool lca)
    {
        lightControlActive = lca;
    }

    public float GetDistanceFromPlayer()
    {
        if (inView) return 0;
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public void SetThisLightIsOnEvent(bool onEvent)
    {
        thisLightOnEvent = onEvent;
    }

    public bool GetThisLightIsOnEvent()
    {
        return thisLightOnEvent;
    }

    public void InPlayerView()
    {
        inView = true;
    }

    public void OutPlayerView()
    {
        inView = false;
    }

    public bool IsInView()
    {
        return inView;
    }
}
