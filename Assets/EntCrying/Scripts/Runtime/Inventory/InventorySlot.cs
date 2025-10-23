using System;

[Serializable]
public class InventorySlot
{
    public Item item;
    public int count;
    public int overlapMaxCount = 9999;
}