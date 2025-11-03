using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    private bool isEnabled;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        isEnabled = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicSystem>().useObjectPooling;
        if (isEnabled) {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for(int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public GameObject InstantiateFromPool(Vector3 position, Quaternion rotation) {
        if (isEnabled) {
            GameObject instance = GetPooledObject();
            if (instance != null) {
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.SetActive(true);
                return instance;
            }
            return null;
        }
        return Instantiate(objectToPool, position, rotation);
    }

    public void destroyInstance(GameObject instance) {
        if (isEnabled) {
            instance.SetActive(false);
        } else {
            Destroy(instance);
        }
    }
}
