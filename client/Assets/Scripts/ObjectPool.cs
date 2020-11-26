using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pool;
    public GameObject pooledObject;
    public int pooledAmmount;
    
    void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < pooledAmmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy) return pool[i];
        }
        return null;
    }

    public GameObject AddObject()
    {
        GameObject obj = Instantiate(pooledObject);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}
