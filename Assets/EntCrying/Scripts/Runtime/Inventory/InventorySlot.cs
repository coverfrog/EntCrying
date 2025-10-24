using System;
using UniRx;
using UnityEngine;

[Serializable]
public class InventorySlot: IDisposable
{
    public int index;
    public Item item;
    public ReactiveProperty<int> count = new();

    private IDisposable _disposableCount;
    
    public InventorySlot(int index, Item item)
    {
        this.index = index;
        this.item = item;

        _disposableCount = count.Subscribe(OnChangeCount);
    }

    public void Dispose()
    {
        _disposableCount?.Dispose();
    }

    private void OnChangeCount(int newCount)
    {
        if (!item)
        {
            return;
        }
        
#if UNITY_EDITOR && true
        Debug.Log($"[INVENTORY] change item count, index: {index}, item: {item.CodeName}, count: {newCount}");
#endif
    }
}