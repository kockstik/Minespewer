using UnityEngine;

public class DarkHoleDamager : MonoBehaviour, IDamager
{
    [SerializeField]
    public int damage { get; private set; } = 1;

    [SerializeField] public Entity sender { get; set; }

    public void SetDamage(IDamager damager)
    {
        damage = damager.damage;
        sender = damager.sender;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hp = other.GetComponentInParent<Health>();
        if (!hp)
            return;

        hp.SetDamage(this);
    }
}
