using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<Minespewer> activeMR = new List<Minespewer>();
    private List<Minespewer> inactiveMR = new List<Minespewer>();

    private SpawnerMR spawner;

    void Start()
    {
        spawner = FindAnyObjectByType<SpawnerMR>();
    }

    public void OnEnableEntity(Entity entity)
    {
        if (entity is Minespewer minespewer)
            activeMR.Add(minespewer);
    }

    public void OnDisableEntity(Entity entity)
    {
        if (entity is Minespewer minespewer)
        {
            activeMR.Remove(minespewer);
            inactiveMR.Add(minespewer);

            if (entity is Bot bot)
                spawner.AddPooledBot(bot);
        }
    }
}
