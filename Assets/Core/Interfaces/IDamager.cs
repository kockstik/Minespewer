using UnityEngine;

public interface IDamager
{
    Entity sender { get; }
    Transform transform { get; }
    int damage { get; }
}
