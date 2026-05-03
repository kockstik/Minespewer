using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class DeathExplosion : MonoBehaviour
{
    [SerializeField] private float forceImpulse = 1000f;
    private float radius = 0;

    void Start()
    {
        radius = GetComponent<SphereCollider>().radius;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == null || !other.transform.parent.TryGetComponent(out Minespewer entity))
            return;

        var rb = entity.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Entity has no Rigidbody component.");
            return;
        }

        var dist = Vector3.Distance(transform.position, entity.transform.position);
        var force = Mathf.Clamp01((radius - dist) / radius);
        Vector3 explosionDirection = (entity.transform.position - transform.position).normalized;
        rb.AddForce(explosionDirection * -forceImpulse * force * Time.deltaTime);
    }

    private void DeleteOnEndAnimation()
    {
        Destroy(gameObject);
    }
}
