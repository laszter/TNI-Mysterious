using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CombineItem : MonoBehaviour {

    public GameObject[] ingredients;
    private bool[] inArea;
    public GameObject result;

	// Use this for initialization
	void Start () {
        inArea = new bool[ingredients.Length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider coll)
    {
        for(int i = 0; i < ingredients.Length; i++)
        {
            if(ingredients[i] == coll.gameObject && coll.isTrigger)
            {
                inArea[i] = true;
                CheckIngredients();
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (ingredients[i] == coll.gameObject)
            {
                inArea[i] = false;
            }
        }
    }

    void CheckIngredients()
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (!inArea[i])
            {
                return;
            }
        }

        GameObject ins = Instantiate(result);
        ins.transform.position = this.transform.position;
        ins.transform.rotation = this.transform.rotation;

        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].SetActive(false);
        }
    }
}
