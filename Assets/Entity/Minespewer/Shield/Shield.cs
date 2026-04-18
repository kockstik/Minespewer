using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Transform sprite;
    private Material spriteMaterial;
    private Color spriteColor;
    [SerializeField] private Transform shield;
    private Material shieldeMaterial;
    private Color shieldColor;

    [SerializeField] private AnimationCurve spriteOpacity;
    [SerializeField] private AnimationCurve shieldOpacity;
    [SerializeField] private float speedAnimation = 0.1f;
    private float time = 0;
    private int maxHealth = 1;
    private int currentHealth = 1;

    private MsHealth health;

    void Start()
    {
        health = GetComponentInParent<MsHealth>();

        spriteMaterial = sprite.GetComponentInChildren<Renderer>().material;
        spriteColor = spriteMaterial.color;
        shieldeMaterial = shield.GetComponent<Renderer>().material;
        shieldColor = shieldeMaterial.color;

        SetOpacity(0);

        health.OnDamage += OnDamage;
        health.OnChangeMaxHealth += OnChangeMaxHealth;
        health.OnChangeHealth += OnChangeHealth;
    }

    private void OnChangeHealth(int health, int? lastHealth)
    {
        currentHealth = health;
        if (health <= 1)
            SetOpacity(0);
    }

    private void OnChangeMaxHealth(int health, int? lastHealth = null)
    {
        maxHealth = health;
    }

    private void OnDamage(Bullet bullet)
    {
        shield.transform.LookAt(bullet.transform);
        time = 0;
    }

    void Update()
    {
        if (maxHealth <= 1 || currentHealth <= 1)
            return;

        time += speedAnimation * Time.deltaTime;
        if (time > 1)
            return;

        spriteColor.a = spriteOpacity.Evaluate(time);
        spriteMaterial.color = spriteColor;

        shieldColor.a = shieldOpacity.Evaluate(time);
        shieldeMaterial.color = shieldColor;
    }

    private void SetOpacity(float opacity)
    {
        spriteColor.a = opacity;
        spriteMaterial.color = spriteColor;

        shieldColor.a = opacity;
        shieldeMaterial.color = shieldColor;
    }
}
