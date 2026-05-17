using UnityEngine;

public class MortarRotator : MonoBehaviour
{
    private Weapone weapone;

    void Start()
    {
        weapone = GetComponent<Weapone>();
    }

    void Update()
    {
        if (!weapone.GetTarget())
            return;

        var pos = new Vector3(weapone.GetTarget().position.x, transform.position.y, weapone.GetTarget().transform.position.z);
        if (Vector3.Distance(pos, transform.position) < 2)
            return;
        transform.LookAt(pos);
    }
}
