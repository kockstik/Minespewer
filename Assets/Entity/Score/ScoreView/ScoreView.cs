using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Fade))]
public class ScoreView : MonoBehaviour, IFadeableObject
{
    private Accounting accounting;
    private Fade fade;
    private TextMeshPro tmpUGUI;

    void Start()
    {
        fade = GetComponent<Fade>();
        tmpUGUI = GetComponent<TextMeshPro>();
        accounting = GetComponentInParent<Accounting>();
        accounting.OnChangeScore += OnChangeScore;
    }

    private void OnChangeScore(int score, int maxScore)
    {
        tmpUGUI.text = $"{score}/{maxScore}";
        fade.TriggerFade(this);
    }

    public void SetAlpha(float a)
    {
        var c = tmpUGUI.faceColor;
        c.a = Byte.Parse(Mathf.RoundToInt(a * 255).ToString());
        tmpUGUI.faceColor = c;
    }
}
