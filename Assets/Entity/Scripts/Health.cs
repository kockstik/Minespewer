using UnityEngine;

public abstract class Health : MonoBehaviour
{
    protected Entity entity;

    public delegate void OnChangeHealth_EventHalder(int health, int? lastHealth = null);
    public OnChangeHealth_EventHalder OnChangeHealth;

    private int _health = 1;
    public int health
    {
        get => _health;
        set
        {
            int? lastHealth = _health;
            _health = value;

            if (OnChangeHealth != null) OnChangeHealth(_health, lastHealth);
        }
    }

    public abstract void SetDamage(IDamager damager);
    protected virtual void OnDie(Entity? killer = null)
    {
        killer.OnKill(entity);
        entity.OnDie(killer);
    }

    void Start()
    {
        entity = GetComponent<Entity>();
        OnStart();
    }

    protected virtual void OnStart() { }
}
