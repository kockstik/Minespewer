using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Weapone))]
public class Mortar : MonoBehaviour
{
    private ShootStatus status;
    private float currentCD = 0;
    [SerializeField] private float Cooldown = 0.3f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform mortarModel;
    [SerializeField] private AnimationCurve targetHeight;

    private Entity owner;
    private Rigidbody ownerRb;

    private Animator animator;
    private Weapone weapone;

    public delegate void OnChangeShootStatus_EventHalder(ShootStatus status);
    public OnChangeShootStatus_EventHalder OnChangeShootStatus;

    void Start()
    {
        weapone = GetComponent<Weapone>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!weapone.GetTarget())
            return;
        if (status == ShootStatus.Reload)
        {
            currentCD = Mathf.Clamp01(currentCD + Time.deltaTime * Cooldown);
            if (currentCD == 1)
                ChangeStatus(ShootStatus.Wait);
        }

        float angle = CalcAngle(shootPoint.position, weapone.GetTarget().position);
        Quaternion quat = new Quaternion();
        quat.eulerAngles = new Vector3(angle - 90, 180, 0);
        mortarModel.localRotation = quat;
    }

    private void ChangeStatus(ShootStatus newStatus)
    {
        status = newStatus;
        if (OnChangeShootStatus != null)
            OnChangeShootStatus(newStatus);
    }

    public void Shoot()
    {
        if (status != ShootStatus.Wait)
            return;

        ChangeStatus(ShootStatus.Shoot);
        animator.SetTrigger("shoot");
    }

    private void OnShoot()
    {
        var bulletGO = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        var rb = bulletGO.GetComponent<Rigidbody>();
        bulletGO.GetComponent<Bullet>().sender = owner;

        var targetPosition = weapone.GetShootPoint();

        float angle = CalcAngle(shootPoint.position, targetPosition);
        float speed = CalcForce(shootPoint.position, targetPosition);

        Vector3 dirXZ = (new Vector3(
            targetPosition.x - shootPoint.position.x,
            0f,
            targetPosition.z - shootPoint.position.z
        )).normalized;

        Vector3 v0 = dirXZ * (speed * Mathf.Cos(angle * Mathf.Deg2Rad));
        v0.y = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

        rb.linearVelocity = v0;// + ownerRb.linearVelocity;
        rb.angularVelocity = UnityEngine.Random.onUnitSphere * speed / 5;

        if (shootEffect != null)
            shootEffect.Play();

        currentCD = 0;
        ChangeStatus(ShootStatus.Reload);
    }

    private float CalcAngle(Vector3 start, Vector3 target)
    {
        float g = Mathf.Abs(Physics.gravity.y);

        Vector3 to = target - start;
        float dy = to.y;
        Vector3 toXZ = new Vector3(to.x, 0f, to.z);
        float d = toXZ.magnitude;

        if (d < 1e-4f) return 45f;

        float H = Mathf.Max(getMinApexMeters(Vector3.Distance(start, target)), dy);
        float vy0 = Mathf.Sqrt(2f * g * H);
        float disc = vy0 * vy0 - 2f * g * dy;
        float t = (vy0 + Mathf.Sqrt(disc)) / g;

        float vxz = d / t;

        return Mathf.Atan2(vy0, vxz) * Mathf.Rad2Deg;
    }

    private float CalcForce(Vector3 start, Vector3 target)
    {
        float g = Mathf.Abs(Physics.gravity.y);

        Vector3 to = target - start;
        float dy = to.y;
        Vector3 toXZ = new Vector3(to.x, 0f, to.z);
        float d = toXZ.magnitude;

        if (d < 1e-4f) return 0f;

        float H = Mathf.Max(getMinApexMeters(Vector3.Distance(start, target)), dy);
        float vy0 = Mathf.Sqrt(2f * g * H);
        float disc = vy0 * vy0 - 2f * g * dy;
        float t = (vy0 + Mathf.Sqrt(disc)) / g;

        float vxz = d / t;
        return Mathf.Sqrt(vxz * vxz + vy0 * vy0);
    }

    private float getMinApexMeters(float distancce)
    {
        return targetHeight.Evaluate(distancce);
    }

    public void SetOwner(Entity entity)
    {
        owner = entity;
        ownerRb = entity.GetComponent<Rigidbody>();
    }

    public float GetCooldown()
    {
        return currentCD;
    }
}

public enum ShootStatus
{
    Reload,
    Wait,
    Shoot
}