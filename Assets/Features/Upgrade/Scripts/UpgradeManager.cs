using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Transform baseModelParent;
    [SerializeField] private Transform mortarModelParent;
    [SerializeField] private Shield shield;

    private GameObject defaultBaseModel;
    private GameObject defaultMortarModel;

    public void Start()
    {
        if (baseModelParent.childCount > 0)
            defaultBaseModel = baseModelParent.GetChild(0).gameObject;
        if (mortarModelParent.childCount > 0)
            defaultMortarModel = mortarModelParent.GetChild(0).gameObject;
    }

    public void SetBaseModel(GameObject baseModel)
    {
        var newBase = Instantiate(baseModel, baseModelParent);
        newBase.transform.localPosition = Vector3.zero;
        defaultBaseModel.SetActive(false);
    }

    public void SetMortarModel(GameObject mortarModel)
    {
        var newMortar = Instantiate(mortarModel, mortarModelParent);
        newMortar.transform.localPosition = Vector3.zero;
        defaultMortarModel.SetActive(false);
    }
}
