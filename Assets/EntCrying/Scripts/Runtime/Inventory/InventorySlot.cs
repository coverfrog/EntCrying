using System;
using UniRx;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public int index;
    public Item item;
    public ReactiveProperty<int> count = new();

    public InventorySlot(int index, Item item)
    {
        this.index = index;
        this.item = item;
    }
}