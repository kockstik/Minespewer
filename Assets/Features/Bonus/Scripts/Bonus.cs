using UnityEngine;

[RequireComponent(typeof(BonusHealth))]
public class Bonus : Entity
{
    [SerializeField] private int killAward = 200;

    public override int GetKillAward()
    {
        return killAward;
    }

    public Vector3 GetPosition() =>
        transform.position;

    public override void OnDie(Entity killer)
    {
        var scorePoints = ObjectPool.shared.GetPooledObject().GetComponent<ScorePoints>();
        scorePoints.transform.position = transform.position;
        scorePoints.StartTo(killer);
        Destroy(gameObject);
    }

    protected override void OnStart()
    {

    }
}
