using Unity.VisualScripting;
using UnityEngine;

public class Accounting : MonoBehaviour
{
    [HideInInspector] public int level = 1;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int scoreToNextLvl = 0;
    private readonly int upLvlMultipler = 100;

    [HideInInspector] public int points = 0;

    private int additiveScore = 0;
    private int fraction = 10;
    [SerializeField] private float delay = 0.3f;
    private float lastAccuredTime = 0;

    private MsHealth health;

    public delegate void OnChangeScore_EventHalder(int score, int maxScore);
    public OnChangeScore_EventHalder OnChangeScore;

    void Start()
    {
        health = GetComponent<MsHealth>();
        score = 0;
        scoreToNextLvl = level * level * upLvlMultipler;
        if (OnChangeScore != null) OnChangeScore(score, scoreToNextLvl);
    }

    void FixedUpdate()
    {
        if (Time.time - lastAccuredTime < delay)
            return;

        if (additiveScore <= 0)
            return;

        var fract = Mathf.Min(additiveScore, fraction);
        score += fract;
        additiveScore -= fract;

        while (score >= scoreToNextLvl)
        {
            level++;
            points++;
            health.SetMaxHealth(level);
            score -= scoreToNextLvl;
            scoreToNextLvl = level * level * upLvlMultipler;
        }
        if (OnChangeScore != null) OnChangeScore(score, scoreToNextLvl);
    }

    public void UpScore(int value)
    {
        additiveScore += value;
    }
}
