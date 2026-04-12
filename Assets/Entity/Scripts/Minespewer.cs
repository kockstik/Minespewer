using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(MsHealth))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Minespewer : Entity
{
    [SerializeField] private DeathExplosion deathExplosionPrefab;
    [SerializeField] private GameObject corpsePrefab;

    [HideInInspector] public int level = 1;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int scoreToNextLvl = 0;
    private readonly int upLvlMultipler = 100;
    [HideInInspector] public int points = 0;
    private MsHealth health;

    public delegate void OnChangeScore_EventHalder(int score, int maxScore);
    public OnChangeScore_EventHalder OnChangeScore;

    protected override void OnStart()
    {
        health = GetComponent<MsHealth>();
        score = 0;
        scoreToNextLvl = level * level * upLvlMultipler;
        GetComponentInChildren<Mortar>().SetOwner(this);
    }

    public override int GetKillAward()
    {
        return scoreToNextLvl / 2;
    }

    public override void OnKill(Entity enemy)
    {
        upScore(enemy.GetKillAward());
    }

    public override void OnDie(Entity killer)
    {
        Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void upScore(int value)
    {
        score += value;
        while (score >= scoreToNextLvl)
        {
            level++;
            points++;
            health.SetMaxHealth(level);
            score -= scoreToNextLvl;
            scoreToNextLvl = level * level * upLvlMultipler;
            //Up level animation
        }
        if (OnChangeScore != null) OnChangeScore(score, scoreToNextLvl);
    }

    public override Vector3 GetVelocity()
    {
        return GetComponent<Rigidbody>().linearVelocity;
    }
}
