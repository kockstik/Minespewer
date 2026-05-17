using UnityEngine;

public class SightPosition : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        camera = Camera.main;

        Cursor.visible = false;
    }

    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Vector3 groundPoint = hit.point;
            transform.position = groundPoint;
        }
    }
}
