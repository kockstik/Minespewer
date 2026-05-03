using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotTargetManager : MonoBehaviour
{
    private List<Entity> enemys = new List<Entity>();
    [SerializeField] private float delay = 3;
    private float timeLastGet = -10;

    [HideInInspector] public Entity target;

    public delegate void OnChangeTarget_EventHalder(Entity target);
    public OnChangeTarget_EventHalder OnChangeTarget;

    void Update()
    {
        if (Time.time - timeLastGet < delay)
            return;
        timeLastGet = Time.time;

        UpdateTarget();
    }

    void UpdateTarget()
    {
        var newTarget = GetTarget();
        if (newTarget != target && OnChangeTarget != null)
            OnChangeTarget(newTarget);
        target = newTarget;
    }

    private Entity GetTarget()
    {
        Entity leader = null;
        float distance = float.MaxValue;

        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == null)
            {
                enemys.Remove(enemys[i]);
                i--;
                continue;
            }

            var dist = Vector3.Distance(transform.position, enemys[i].transform.position);
            if (dist > distance)
                continue;

            distance = dist;
            leader = enemys[i];
        }

        return leader;
    }

    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Entity>();
        if (enemy == null || enemys.Contains(enemy))
            return;

        enemy.Destroyed += OnTargetDie;
        enemys.Add(enemy);

        if (enemys.Count == 1)
            UpdateTarget();
    }

    void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponentInParent<Entity>();
        if (enemy == null)
            return;

        if (!enemys.Contains(enemy))
            return;

        enemy.Destroyed -= OnTargetDie;
        enemys.Remove(enemy);
    }

    void OnTargetDie(Entity entity)
    {
        entity.Destroyed -= OnTargetDie;
        if (enemys.Contains(entity))
            enemys.Remove(entity);
    }
}
