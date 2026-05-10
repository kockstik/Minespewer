using UnityEngine;

public class HidingAreaControl : MonoBehaviour
{
    [SerializeField] private float radius = 8;

    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent.TryGetComponent(out Minespewer minespewer))
            return;

        var setter = minespewer.GetComponentInChildren<HidingAreaSetter>();
        if (setter == null)
            return;

        setter.SetArea(transform.position, radius);
    }
}
