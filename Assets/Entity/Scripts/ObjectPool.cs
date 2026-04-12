using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool shared;
    private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    void Awake()
    {
        shared = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        return pooledObjects.FirstOrDefault(o => o.activeInHierarchy == false);
    }
}
