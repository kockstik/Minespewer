using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HidingAreaSetter : MonoBehaviour
{
    private List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>(true)
            .Where(m => m.sharedMaterial != null && m.sharedMaterial.shader.name == "Custom/MRStencil")
            .ToList();
    }

    public void SetArea(Vector3 position, float radius)
    {
        foreach (var renderer in renderers)
        {
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetVector("_HideCenter", position);
            block.SetFloat("_HideRadius", radius);
            renderer.SetPropertyBlock(block);
        }
    }
}
