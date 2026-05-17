using System.Collections.Generic;
using UnityEngine;

public class BotMarkerator : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var bot = other.GetComponentInParent<Bot>();
        if (!bot)
            return;

        var marker = bot.GetComponentInChildren<BotMarker>();
        if (!marker)
            return;

        marker.Show();
    }

    void OnTriggerExit(Collider other)
    {
        var bot = other.GetComponentInParent<Bot>();
        if (!bot)
            return;

        var marker = bot.GetComponentInChildren<BotMarker>();
        if (!marker)
            return;

        marker.Hide();
    }
}
