using UnityEngine;

public class BotSight : MonoBehaviour
{
    private Entity target;

    [SerializeField] private float smooth = 5;

    void Update()
    {
        if (target == null)
            return;

        var lerp = smooth * Time.deltaTime;
        var targetPosition = target.transform.position + target.GetVelocity() * 2;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerp);
    }

    public void SetTarget(Entity enemy)
    {
        target = enemy;
    }
}