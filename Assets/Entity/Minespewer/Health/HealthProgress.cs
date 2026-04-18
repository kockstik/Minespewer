using System;
using UnityEngine;

[RequireComponent(typeof(Fade))]
public class HealthProgress : MonoBehaviour, IFadeableObject
{
    [SerializeField]
    private GameObject progressObj;
    [SerializeField]
    private GameObject borderObj;
    private Material progressMaterial;
    private Material borderMaterial;
    private Color progressBaseColor;
    private Color borderBaseColor;
    private MsHealth playerHealth;
    private Fade fade;

    private int maxHealth;
    private int currentHealth;

    void Start()
    {
        fade = GetComponent<Fade>();
        playerHealth = GetComponentInParent<MsHealth>();
        maxHealth = playerHealth.GetMaxHealth();
        currentHealth = playerHealth.GetHealth();

        playerHealth.OnChangeHealth += OnChangeHealth;
        playerHealth.OnChangeMaxHealth += OnChangeMaxHealth;

        progressMaterial = progressObj.GetComponentInChildren<Renderer>().material;
        borderMaterial = borderObj.GetComponent<Renderer>().material;

        progressBaseColor = progressMaterial.color;
        borderBaseColor = borderMaterial.color;

        UpdateProgress();
    }

    private void OnChangeMaxHealth(int health, int? lastHealth = null)
    {
        maxHealth = health;
        UpdateProgress();
    }

    private void OnChangeHealth(int health, int? lastHealth = null)
    {
        currentHealth = health;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        if (maxHealth <= 0 || progressObj == null || progressMaterial == null)
            return;

        float fill = Mathf.Clamp01((float)currentHealth / maxHealth);

        var t = progressObj.transform;
        var scale = t.localScale;
        scale.x = fill;
        t.localScale = scale;

        float tilingX = Mathf.Max(fill, 0.0001f);
        Vector2 tiling = progressMaterial.mainTextureScale;
        tiling.x = tilingX;
        progressMaterial.mainTextureScale = tiling;

        fade.TriggerFade(this);
    }

    public void SetAlpha(float a)
    {
        var c1 = progressBaseColor;
        c1.a = a;
        progressMaterial.color = c1;

        var c2 = borderBaseColor;
        c2.a = a;
        borderMaterial.color = c2;
    }
}
