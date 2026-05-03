using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float checkupDelay = 10f;
    private float lastCheckup = 0f;

    private Queue<Minespewer> toSpawn = new Queue<Minespewer>();
    private bool isQueueProcessing = false;
    private List<Minespewer> entityes = new List<Minespewer>();

    public bool canSpawn => entityes.Count <= 0;

    public void AddSpawn(Minespewer entity)
    {
        toSpawn.Enqueue(entity);
        if (!isQueueProcessing)
            StartCoroutine(ProcessSpawnQueue());
    }

    public void AddSpawnImmediate(Minespewer entity)
    {
        if (canSpawn)
            spawn(entity);
        else
            toSpawn.Enqueue(entity);
    }

    private void spawn(Minespewer entity)
    {
        entity.transform.position = transform.position;
        entity.gameObject.SetActive(true);
        Add(entity);
    }

    void Update()
    {
        Debug.Log($"SpawnPoint: {entityes.Count} entities, {toSpawn.Count} in queue");
        if (Time.time - lastCheckup > checkupDelay && entityes.Count == 1)
        {
            lastCheckup = Time.time;
            foreach (var entity in entityes)
            {
                if (!entity.gameObject.activeInHierarchy)
                    Remove(entity);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out Minespewer entity))
            Add(entity);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out Minespewer entity))
            Remove(entity);

        if (canSpawn && toSpawn.Count > 0)
        {
            if (!isQueueProcessing)
                StartCoroutine(ProcessSpawnQueue());
        }
    }

    void Destroyed(Entity entity)
    {
        if (entity is Minespewer mr)
            Remove(mr);
    }

    private void Add(Minespewer entity)
    {
        if (entityes.Contains(entity))
            return;
        entity.Destroyed += Destroyed;
        entityes.Add(entity);
    }

    private void Remove(Minespewer entity)
    {
        if (!entityes.Contains(entity))
            return;
        entity.Destroyed -= Destroyed;
        entityes.Remove(entity);
    }

    private IEnumerator ProcessSpawnQueue()
    {
        isQueueProcessing = true;
        while (toSpawn.Count > 0)
        {
            var entity = toSpawn.Dequeue();

            while (!canSpawn)
                yield return null;

            spawn(entity); ;
            yield return new WaitForSeconds(spawnDelay);
        }
        isQueueProcessing = false;
    }
}
