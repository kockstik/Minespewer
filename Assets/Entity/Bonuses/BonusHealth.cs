using UnityEngine;

public class BonusHealth : Health
{
    public override void SetDamage(Bullet bullet)
    {
        OnDie(bullet.sender);
    }
}
