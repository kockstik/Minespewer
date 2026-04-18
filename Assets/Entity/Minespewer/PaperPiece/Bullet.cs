using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject groundExplosion;
    [SerializeField] private GameObject entityExplosion;
    [SerializeField] private Transform shadow;
    [SerializeField] private float lifetime = 8f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask damageMask;

    [HideInInspector] public int damage = 1;
    [HideInInspector] public Entity sender;

    private readonly String GROUND_TAG = "Ground";

    private bool isExploded = false;

    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
        shadow.transform.parent = null;
        shadow.transform.localEulerAngles = Vector3.zero;
    }

    void Update()
    {
        if (Time.time - spawnTime > lifetime)
            Destroy();

        shadow.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isExploded)
            return;
        isExploded = true;

        var hits = Physics.OverlapSphere(transform.position, explosionRadius, damageMask);
        var hitHealth = new List<Health>();

        foreach (var h in hits)
        {
            var hp = h.GetComponentInParent<Health>();
            if (!hp)
                continue;
            if (hitHealth.Contains(hp))
                continue;

            hp.SetDamage(this);
            hitHealth.Add(hp);
        }

        if (collision.gameObject.tag == GROUND_TAG)
            AddGroundExplosion();
        else
            AddEntityExplosion(collision.contacts[0].normal);

        Destroy();
    }

    private void AddGroundExplosion()
    {
        var pos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        Instantiate(groundExplosion, pos, Quaternion.identity);
    }

    private void AddEntityExplosion(Vector3 direction)
    {
        Instantiate(entityExplosion, transform.position, Quaternion.LookRotation(direction));
    }

    private void Destroy()
    {
        Destroy(shadow.gameObject);
        Destroy(gameObject);
    }
}
