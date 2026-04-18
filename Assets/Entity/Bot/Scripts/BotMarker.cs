using UnityEngine;

public class BotMarker : MonoBehaviour
{
    [SerializeField] private GameObject marker;

    public void Show()
    {
        marker.SetActive(true);
    }

    public void Hide()
    {
        marker.SetActive(false);
    }
}
