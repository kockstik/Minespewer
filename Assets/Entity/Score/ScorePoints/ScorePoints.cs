using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScorePoints : MonoBehaviour
{
    private List<PointData> points;
    [SerializeField] private GameObject point;
    [SerializeField] private int amount = 5;
    [SerializeField] private float lifetime = 3f;
    private float startTime;
    [SerializeField] private AnimationCurve speedByLifetime;
    [SerializeField] private AnimationCurve directionByLifetime;

    private Entity entity;

    void Awake()
    {
        points = new List<PointData>();
        GameObject tmp;
        for (int i = 0; i < amount; i++)
        {
            tmp = Instantiate(point);
            tmp.SetActive(false);
            tmp.transform.parent = transform;
            points.Add(new PointData { obj = tmp, target = Vector3.zero });
        }
    }

    void OnEnable()
    {
        if (entity == null)
            return;

        startTime = Time.time;
        foreach (var p in points)
        {
            p.obj.transform.position = transform.position;
            p.obj.SetActive(true);
            var target = Random.onUnitSphere * 5f + transform.position;
            target.y = Mathf.Abs(target.y);
            p.target = target;
        }
    }

    void FixedUpdate()
    {
        if (entity == null)
        {
            TryDisable();
            return;
        }

        if (Time.time - startTime >= lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        foreach (var kvp in points)
        {
            if (!kvp.obj.activeInHierarchy)
                continue;

            var obj = kvp.obj;
            var playerPos = entity.transform.position;
            var dist = (obj.transform.position - playerPos).sqrMagnitude;
            float speed = speedByLifetime.Evaluate((Time.time - kvp.startTime) / lifetime);

            var target = Vector3.Lerp(kvp.target, playerPos, directionByLifetime.Evaluate((Time.time - kvp.startTime) * Time.fixedDeltaTime * speed));

            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                target,
                speed * Time.fixedDeltaTime);

            if (dist <= 4f)
                obj.SetActive(false);
        }
    }

    public void StartTo(Entity entity)
    {
        this.entity = entity;
        gameObject.SetActive(true);
    }

    private void TryDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            OnDisable();
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        foreach (var kvp in points)
        {
            if (!kvp.obj.activeInHierarchy)
                continue;
            var obj = kvp.obj;
            obj.SetActive(false);
        }
        entity = null;
    }
}
