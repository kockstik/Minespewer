using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public Sprite image;
    public string upgradeName;
    public string description;
    public int price;
    public int minLevel;

    public abstract void ApplyUpgrade(Minespewer mr);
}
