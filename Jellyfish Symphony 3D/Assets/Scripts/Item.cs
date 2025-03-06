using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStack = 99;
    public bool isStackable;
    public string itemDescription;

    public enum ItemType { Weapon, Artifact, Material, Consumable }
}