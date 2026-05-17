using UnityEngine;

public class Fade : MonoBehaviour
{
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

    private IFadeableObject fadeableObject;

    void Update()
    {
        if (fadeableObject == null)
            return;

        idleTimer += Time.deltaTime;

        if (isFadingIn)
        {
            fadeInTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeInTimer / fadeInDuration);
            currentAlpha = Mathf.Lerp(fadeInStartAlpha, 1f, t);
            fadeableObject.SetAlpha(currentAlpha);

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
            fadeableObject.SetAlpha(currentAlpha);

            if (t >= 1f)
            {
                isFadingOut = false;
            }
        }
    }

    public void TriggerFade(IFadeableObject fadeableObject)
    {
        if (this.fadeableObject == null)
            this.fadeableObject = fadeableObject;

        idleTimer = 0f;
        isFadingOut = false;
        fadeOutTimer = 0f;

        isFadingIn = true;
        fadeInTimer = 0f;
        fadeInStartAlpha = currentAlpha;
    }
}
