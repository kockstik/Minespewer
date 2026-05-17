using UnityEngine;

class PointData
{
    public GameObject obj;

    private Vector3 _target;
    public Vector3 target
    {
        get => _target;
        set
        {
            _target = value;
            startDistance = (obj.transform.position - _target).sqrMagnitude;
            startTime = Time.time;
        }
    }

    public float startDistance { get; private set; }
    public float startTime { get; private set; }
}