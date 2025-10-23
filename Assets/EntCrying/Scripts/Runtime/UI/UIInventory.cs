using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public class Va
    {
        
    }
    
    [SerializeField] private List<UIInventorySlot> _inventorySlotList = new();

    private InventorySlot _slot;
    
    public void Initialize()
    {
        foreach (UIInventorySlot slot in _inventorySlotList)
        {
            slot.Set(null);
        }
    }
    
    public void OnAdd(int idx, InventorySlot slot)
    {
        // 데이터
        _slot = slot;
        
        // TODO : 인벤토리도 동적으로 생성 필요
        // 현재는 빠른 테스팅을 위해 고정
        
        // 적용
        _inventorySlotList[idx].Set(slot);
        
    }

    public void OnReplace(Item item, int oldCount, int newCount)
    {
        
    }

    public void OnCountChange(int count)
    {
        
    }
}
