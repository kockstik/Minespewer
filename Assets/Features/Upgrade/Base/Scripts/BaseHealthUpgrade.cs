using UnityEngine;

[CreateAssetMenu(fileName = "BaseHealthUpgrade", menuName = "Upgrades/Base Health")]
public class BaseHealthUpgrade : Upgrade
{
    [SerializeField] private GameObject upgradedModel;

    public override void ApplyUpgrade(Minespewer mr)
    {
        var health = mr.GetComponent<Health>();
        if (health != null)
            health.AddMaxHealth(1);
        else
            Debug.LogError("BaseHealthUpgrade: Health component not found on Minespewer.");

        if (upgradedModel != null)
        {
            var upgradeManager = mr.GetComponent<UpgradeManager>();
            if (upgradeManager != null)
                upgradeManager.SetBaseModel(upgradedModel);
            else
                Debug.LogError("BaseHealthUpgrade: UpgradeManager component not found on Minespewer.");
        }
    }
}
