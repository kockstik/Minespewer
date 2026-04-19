using UnityEngine;

public class BonusHealth : Health
{
    public override void SetDamage(IDamager damager)
    {
        OnDie(damager.sender);
    }
}
