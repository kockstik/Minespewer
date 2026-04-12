using System;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    private MsHealth playerHealth;
    private ParticleSystem particles;

    private void Start()
    {
        playerHealth = GetComponentInParent<MsHealth>();
        particles = GetComponent<ParticleSystem>();

        playerHealth.OnChangeHealth += OnChangeHealth;
    }

    private void OnChangeHealth(int health, int? lastHealth = null)
    {
        if (lastHealth == null || health <= lastHealth)
            return;

        particles.Play();
    }
}
