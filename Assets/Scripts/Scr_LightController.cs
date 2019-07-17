using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LightController : MonoBehaviour {

    private GameObject[] neonLights;
    private GameObject player;
    [SerializeField] List<float> lightDistance;
    [SerializeField] List<GameObject> generalFarLight;
    [SerializeField] private int countActive;
    private int maximumLight = 4;

	// Use this for initialization
	void Start () {
        neonLights = GameObject.FindGameObjectsWithTag("Light");
        player = GameObject.FindGameObjectWithTag("Player");
        countActive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        countActive = 0;
        foreach(GameObject neonLight in neonLights)
        {
            if (neonLight.GetComponent<Scr_Light>().IsLightActive())
            {
                countActive += 1;
                //Debug.Log(neonLight.GetComponent<Scr_Light>().GetDistanceFromPlayer() + "\n");
            }
        }

        if(countActive > 3)
        {
            //float tempDistance = 0;
            generalFarLight = new List<GameObject>();
            generalFarLight.Clear();
            lightDistance = new List<float>();
            lightDistance.Clear();
            foreach (GameObject neonLight in neonLights)
            {
                if (neonLight.GetComponent<Scr_Light>().IsLightActive() && !neonLight.GetComponent<Scr_Light>().GetThisLightIsOnEvent())
                {
                    generalFarLight.Add(neonLight);
                }
            }

            // Re Order General Light from near to far
            for (int j = 0; j < generalFarLight.Count - 1; j++)
            {
                for (int i = 0; i < generalFarLight.Count - 1; i++)
                {
                    if (generalFarLight[i].GetComponent<Scr_Light>().GetDistanceFromPlayer() > generalFarLight[i + 1].GetComponent<Scr_Light>().GetDistanceFromPlayer())
                    {
                        GameObject temp = generalFarLight[i + 1];
                        generalFarLight[i + 1] = generalFarLight[i];
                        generalFarLight[i] = temp;
                    }
                }
            }
            //------------------- Continue Optimize General Light ------------------------------
            for (int i = 0; i < maximumLight - 1; i++)
            {
                generalFarLight[i].GetComponent<Scr_Light>().SetLightControlActive(true);
                lightDistance.Add(generalFarLight[i].GetComponent<Scr_Light>().GetDistanceFromPlayer());
                //Debug.Log(farLight[i].GetComponent<Scr_Light>().GetDistanceFromPlayer() + " " + i + " : \n");
            }
            for (int i = maximumLight; i < generalFarLight.Count; i++)
            {
                generalFarLight[i].GetComponent<Scr_Light>().SetLightControlActive(false);
                lightDistance.Add(generalFarLight[i].GetComponent<Scr_Light>().GetDistanceFromPlayer());
                //Debug.Log(farLight[i].GetComponent<Scr_Light>().GetDistanceFromPlayer() + " " + i + " : \n");
            }
        }
    }
}
