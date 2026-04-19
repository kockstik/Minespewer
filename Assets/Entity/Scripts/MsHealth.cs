using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;

public class MsHealth : Health
{
    private Rigidbody entityRb;
    private int maxHealth = 1;

    [SerializeField] private float impulseOnDamage = 20;

    private float timeLastGetDamage = 0;
    [SerializeField] private float delayToHeal = 10;
    [SerializeField] private float delayHeal = 1;
    private float timeLastHeal = 0;

    public OnChangeHealth_EventHalder OnChangeMaxHealth;
    public delegate void OnDamage_EventHalder(IDamager damager);
    public OnDamage_EventHalder OnDamage;

    protected override void OnStart()
    {
        entityRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.time - timeLastGetDamage < delayToHeal || health >= maxHealth)
            return;

        Heal();
    }

    private void Heal()
    {
        if (Time.time - timeLastHeal < delayHeal)
            return;

        timeLastHeal = Time.time;
        health = Mathf.Clamp(health + 1, 0, maxHealth);
    }

    public override void SetDamage(IDamager damager)
    {
        if (OnDamage != null) OnDamage(damager);

        var impulseVelocity = (transform.position - damager.transform.position).normalized * impulseOnDamage;
        impulseVelocity.y = 0;
        entityRb.AddForce(impulseVelocity, ForceMode.Impulse);

        timeLastGetDamage = Time.time;
        health -= damager.damage;

        if (health <= 0)
            OnDie(damager.sender);
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        health = value;
        if (OnChangeMaxHealth != null) OnChangeMaxHealth(maxHealth);
    }

    public int GetMaxHealth() => maxHealth;
    public int GetHealth() => health;
}
