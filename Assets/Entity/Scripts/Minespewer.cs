using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(MsHealth))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Accounting))]
public abstract class Minespewer : Entity
{
    [SerializeField] private DeathExplosion deathExplosionPrefab;
    [SerializeField] private GameObject corpsePrefab;
    private Accounting scoreManager;
    private MsHealth health;

    protected override void OnStart()
    {
        health = GetComponent<MsHealth>();
        scoreManager = GetComponent<Accounting>();
        GetComponentInChildren<Mortar>().SetOwner(this);
    }

    public override int GetKillAward()
    {
        return scoreManager.scoreToNextLvl / 2;
    }

    public override void OnKill(Entity enemy)
    {
        scoreManager.UpScore(enemy.GetKillAward());
    }

    public override void OnDie(Entity killer)
    {
        var damager = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity).GetComponentInChildren<DarkHoleDamager>();
        damager.sender = killer;
        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override Vector3 GetVelocity()
    {
        return GetComponent<Rigidbody>().linearVelocity;
    }
}
