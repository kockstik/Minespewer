using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotController : MonoBehaviour
{
    private NavMeshAgent agent;
    private BotTargetManager targetManager;

    [SerializeField] private float delay = 3;
    private float timeLastUpdate = -10;

    private Movement movement;
    private Weapone weapone;
    private Mortar mortar;
    private BotSight botSight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetManager = GetComponentInChildren<BotTargetManager>();
        movement = GetComponent<Movement>();
        weapone = GetComponentInChildren<Weapone>();
        mortar = GetComponentInChildren<Mortar>();
        botSight = GetComponentInChildren<BotSight>();

        weapone.SetTarget(botSight.transform);

        targetManager.OnChangeTarget += OnChangeTarget;
        mortar.OnChangeShootStatus += OnChangeShootStatus;
    }

    private void OnChangeShootStatus(ShootStatus status)
    {
        if (status == ShootStatus.Wait)
        {
            mortar.Shoot();
        }
    }

    private void OnChangeTarget(Entity target)
    {
        botSight.SetTarget(target);
    }

    void Update()
    {
        var direction = Vector3.zero;
        if (agent.path.corners.Length > 1)
            direction = agent.path.corners[1] - transform.position;
        movement.Move(direction);

        if (targetManager.target == null)
            return;

        if (Time.time - timeLastUpdate < delay)
            return;
        timeLastUpdate = Time.time;

        var randomInCircle = UnityEngine.Random.insideUnitCircle.normalized * 25;
        var position = targetManager.target.transform.position + new Vector3(randomInCircle.x, 0, randomInCircle.y);

        agent.SetDestination(position);
    }
}
