using System.Collections.Generic;
using UnityEngine;

public class WindmillWindForce : MonoBehaviour
{
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private float force = 300f;

    void FixedUpdate()
    {
        foreach (var rb in rigidbodies)
        {
            if (rb == null || rb.gameObject == null || !rb.gameObject.activeInHierarchy)
                rigidbodies.Remove(rb);
            else
                rb.AddForce(transform.right * force * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null || !other.transform.parent.TryGetComponent(out Minespewer entity))
            return;

        var rb = entity.GetComponent<Rigidbody>();
        if (rb != null && !rigidbodies.Contains(rb))
            rigidbodies.Add(rb);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null || !other.transform.parent.TryGetComponent(out Minespewer entity))
            return;

        var rb = entity.GetComponent<Rigidbody>();
        if (rb != null)
            rigidbodies.Remove(rb);
    }
}
