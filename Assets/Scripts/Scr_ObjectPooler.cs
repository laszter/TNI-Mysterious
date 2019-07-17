using System.Collections.Generic;
using UnityEngine;

public class Scr_ObjectPooler : MonoBehaviour {

    static Scr_ObjectPooler objectPooler;
    public static Scr_ObjectPooler Instance
    {
        get { return objectPooler; }
    }

    private void Awake()
    {
        if (objectPooler == null) objectPooler = this;
    }

    [System.Serializable]
    class PoolingObject
    {
        [SerializeField] public string name;
        [SerializeField] GameObject prefab;
        [SerializeField] int initAmout;

        List<GameObject> pooler;

        public GameObject GetObjectFromPooler()
        {
            if (pooler == null)
            {
                pooler = new List<GameObject>();
            }

            for (int i = 0; i < pooler.Count; i++)
            {
                if (!pooler[i].activeInHierarchy)
                {

                    pooler[i].SetActive(true);
                    return pooler[i];
                }
            }

            GameObject instantiatedObject = Instantiate(prefab);
            pooler.Add(instantiatedObject);
            instantiatedObject.SetActive(true);
            return instantiatedObject;
        }
    }

    [SerializeField] PoolingObject[] poolingObjects;

    public GameObject GetPoolingObject(string name)
    {
        for (int i = 0; i < poolingObjects.Length; i++)
        {
            if (poolingObjects[i].name == name)
            {
                return poolingObjects[i].GetObjectFromPooler();
            }
        }
        return null;
    }
}
