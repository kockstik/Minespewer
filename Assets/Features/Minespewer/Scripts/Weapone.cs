using UnityEngine;

public class Weapone : MonoBehaviour
{
    [SerializeField] private float accuracy = 0.3f;
    public float size = 1;

    private Mortar mortar;

    private Transform target;

    void Start()
    {
        mortar = GetComponent<Mortar>();
    }

    void Update()
    {
        if (!target)
            return;

        var dist = Vector3.Distance(transform.position, target.position);
        size = Mathf.Clamp(dist * accuracy - 1, 1.5f, Mathf.Infinity);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public Vector3 GetShootPoint()
    {
        var randomInCircle = Random.insideUnitCircle * size / 2;
        var point = new Vector3(randomInCircle.x, 0, randomInCircle.y) + target.position;
        return point;
    }
}
