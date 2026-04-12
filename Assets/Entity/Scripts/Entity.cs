using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    void Start()
    {
        OnStart();
    }

    protected abstract void OnStart();
    public abstract int GetKillAward();
    public abstract void OnDie(Entity killer);

    public virtual void OnKill(Entity enemy) { }
    public virtual Vector3 GetVelocity()
    {
        return Vector3.zero;
    }
}
