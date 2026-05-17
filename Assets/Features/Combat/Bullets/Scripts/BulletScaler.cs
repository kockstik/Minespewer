using UnityEngine;

public class BulletScaler : MonoBehaviour
{
    [SerializeField] private float multipler = 0.1f;
    [SerializeField] private Transform shadow;

    void Update()
    {
        var scale = Mathf.Clamp((transform.position.y + 10f) * multipler, 1, Mathf.Infinity);
        transform.localScale = new Vector3(scale, scale, scale);
        shadow.localScale = new Vector3(scale / 8, scale / 8, scale / 8);
    }
}
