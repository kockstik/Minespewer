using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 2f;
    private float lastSpavnTime = 0f;

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
        entity.Destroyed += Destroyed;
        entity.gameObject.SetActive(true);
    }

    void Update()
    {
        Debug.Log($"SpawnPoint: {entityes.Count} entities, {toSpawn.Count} in queue");
        if (entityes.Count > 0)
            Debug.Log(entityes[0]);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out Minespewer entity))
        {
            entity.Destroyed += Destroyed;
            entityes.Add(entity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out Minespewer entity))
        {
            entity.Destroyed -= Destroyed;
            entityes.Remove(entity);
        }

        if (canSpawn && toSpawn.Count > 0)
        {
            if (!isQueueProcessing)
                StartCoroutine(ProcessSpawnQueue());
        }
    }

    void Destroyed(Entity entity)
    {
        if (entity is Minespewer mr)
            entityes.Remove(mr);
        entity.Destroyed -= Destroyed;
    }

    private IEnumerator ProcessSpawnQueue()
    {
        isQueueProcessing = true;
        while (toSpawn.Count > 0)
        {
            var entity = toSpawn.Dequeue();

            while (!canSpawn)
                yield return null;

            spawn(entity);
            lastSpavnTime = Time.time;
            yield return new WaitForSeconds(spawnDelay);
        }
        isQueueProcessing = false;
    }
}
