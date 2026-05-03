using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntityManager? entityManager;

    public event Action<Entity>? Destroyed;

    void Start()
    {
        OnStart();
    }

    void OnEnable()
    {
        entityManager?.OnEnableEntity(this);
    }

    protected abstract void OnStart();
    public abstract int GetKillAward();
    public abstract void OnDie(Entity killer);

    public virtual void OnKill(Entity enemy) { }
    public virtual Vector3 GetVelocity()
    {
        return Vector3.zero;
    }

    internal void Destroy(GameObject gameObject)
    {
        Destroyed?.Invoke(this);
        entityManager?.OnDisableEntity(this);
        gameObject.SetActive(false);
    }
}
