using System;
using UnityEngine;

public class HealthProgress : MonoBehaviour
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

    private int maxHealth;
    private int currentHealth;

    [SerializeField] private float idleDelay = 4f;
    private float idleTimer = 0f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeInDuration = 0.15f;

    private float currentAlpha = 1f;

    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private float fadeInTimer = 0f;
    private float fadeOutTimer = 0f;
    private float fadeInStartAlpha = 0f;

    void Start()
    {
        playerHealth = GetComponentInParent<MsHealth>();
        maxHealth = playerHealth.GetMaxHealth();
        currentHealth = playerHealth.GetHealth();

        playerHealth.OnChangeHealth += OnChangeHealth;
        playerHealth.OnChangeMaxHealth += OnChangeMaxHealth;

        progressMaterial = progressObj.GetComponentInChildren<Renderer>().material;
        borderMaterial = borderObj.GetComponent<Renderer>().material;

        progressBaseColor = progressMaterial.color;
        borderBaseColor = borderMaterial.color;

        currentAlpha = 1f;
        SetAlpha(currentAlpha);


        UpdateProgress();
    }

    void Update()
    {
        idleTimer += Time.deltaTime;

        if (isFadingIn)
        {
            fadeInTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeInTimer / fadeInDuration);
            currentAlpha = Mathf.Lerp(fadeInStartAlpha, 1f, t);
            SetAlpha(currentAlpha);

            fadeOutTimer = 0f;
            isFadingOut = false;

            if (t >= 1f)
            {
                isFadingIn = false;
            }

            return;
        }

        if (idleTimer > idleDelay)
        {
            if (!isFadingOut)
            {
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            fadeOutTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeOutTimer / fadeOutDuration);
            currentAlpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(currentAlpha);

            if (t >= 1f)
            {
                isFadingOut = false;
            }
        }
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

        idleTimer = 0f;
        isFadingOut = false;
        fadeOutTimer = 0f;

        isFadingIn = true;
        fadeInTimer = 0f;
        fadeInStartAlpha = currentAlpha;
    }

    private void SetAlpha(float a)
    {
        var c1 = progressBaseColor;
        c1.a = a;
        progressMaterial.color = c1;

        var c2 = borderBaseColor;
        c2.a = a;
        borderMaterial.color = c2;
    }
}
