using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SightPosition))]
public class Sight : MonoBehaviour
{
    [SerializeField] private int pointsCount = 32;
    [SerializeField] private Transform trigger;

    private LineRenderer line;
    private Player player;
    private Weapone weapone;
    private Mortar mortar;

    private Vector3[] unitCircle;

    private Coroutine pulseRoutine;
    private Coroutine backwardRoutine;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.loop = false;
        line.useWorldSpace = true;

        line.positionCount = pointsCount + 1;

        unitCircle = new Vector3[pointsCount + 1];
        float step = Mathf.PI * 2f / pointsCount;
        for (int i = 0; i <= pointsCount; i++)
        {
            float a = i * step;
            unitCircle[i] = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
        }

        player = FindAnyObjectByType<Player>();
        weapone = player.GetComponentInChildren<Weapone>();
        mortar = player.GetComponentInChildren<Mortar>();
        weapone.SetTarget(transform);

        mortar.OnChangeShootStatus += OnChangeShootStatus;
    }

    private void OnChangeShootStatus(ShootStatus status)
    {
        if (status == ShootStatus.Shoot)
        {
            if (backwardRoutine != null)
                StopCoroutine(backwardRoutine);
            backwardRoutine = StartCoroutine(BackwardSight());
        }
        if (status == ShootStatus.Wait)
        {
            if (pulseRoutine != null)
                StopCoroutine(pulseRoutine);
            pulseRoutine = StartCoroutine(PulseAnimation());
        }
    }

    void Update()
    {
        if (backwardRoutine == null)
            DrawCircle(mortar.GetCooldown());
    }

    private IEnumerator PulseAnimation()
    {
        float duration = 0.2f;
        float t = 0f;
        float startWidth = line.startWidth;
        float targetWidth = startWidth * 2f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float w = Mathf.Lerp(startWidth, targetWidth, t);
            line.startWidth = w;
            line.endWidth = w;
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float w = Mathf.Lerp(targetWidth, startWidth, t);
            line.startWidth = w;
            line.endWidth = w;
            yield return null;
        }
    }

    private IEnumerator BackwardSight()
    {
        float cd = 1;
        float duration = 0.2f;

        while (cd > 0 || mortar.GetCooldown() == 1)
        {
            cd -= Time.deltaTime / duration;
            cd = Mathf.Clamp01(cd);
            DrawCircle(cd);
            yield return null;
        }

        backwardRoutine = null;
    }

    private void DrawCircle(float cooldown)
    {
        var size = weapone.size * transform.localScale.magnitude;
        trigger.localScale = new Vector3(size, size, size);
        float radius = size / 2f;

        int totalPoints = pointsCount + 1;
        if (line.positionCount != totalPoints)
            line.positionCount = totalPoints;

        float t = Mathf.Clamp01(cooldown);
        int lastIndex = totalPoints - 1;
        Vector3 center = transform.position;

        if (t <= 0f)
        {
            for (int i = 0; i <= lastIndex; i++)
                line.SetPosition(i, center);
            return;
        }

        float step = Mathf.PI * 2f / lastIndex;

        if (t >= 1f)
        {
            for (int i = 0; i < lastIndex; i++)
            {
                float a = i * step;
                Vector3 p = center + new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * radius;
                line.SetPosition(i, p);
            }
            line.SetPosition(lastIndex, line.GetPosition(0));
            return;
        }

        float endIdxFloat = t * lastIndex;
        int fullIdx = Mathf.FloorToInt(endIdxFloat);
        float frac = endIdxFloat - fullIdx;

        for (int i = 0; i <= fullIdx; i++)
        {
            float a = i * step;
            Vector3 p = center + new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * radius;
            line.SetPosition(i, p);
        }

        int nextIdx = fullIdx + 1;
        float aFrom = fullIdx * step;
        float aTo = nextIdx * step;
        Vector3 from = center + new Vector3(Mathf.Cos(aFrom), 0f, Mathf.Sin(aFrom)) * radius;
        Vector3 to = center + new Vector3(Mathf.Cos(aTo), 0f, Mathf.Sin(aTo)) * radius;
        Vector3 partial = Vector3.Lerp(from, to, frac);
        line.SetPosition(nextIdx, partial);

        for (int i = nextIdx + 1; i <= lastIndex; i++)
            line.SetPosition(i, partial);
    }
}
