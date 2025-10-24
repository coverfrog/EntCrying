using UnityEngine;

public abstract class Item : IdentifiedObject
{
    [Header("[ ITEM - BASE ]")] 
    
    [SerializeField, Tooltip("획득 하자마자 사용 여부")]
    private bool _isImmediately;
    public bool IsImmediately => _isImmediately;

    [SerializeField, Tooltip("아이템 사용 후 삭제 여부")]
    private bool _isDeleteConsumable = true; 
    public bool IsDeleteConsumable => _isDeleteConsumable;
    
    // ----------------------------------------------------------------------------
    
    [Header("[ ITEM - INVENTORY ]")] 
    
    [SerializeField, Tooltip("슬롯 최대 개수")]
    private int _slotOverlapMaxCount = 9999;
    public int SlotOverlapMaxCount => _slotOverlapMaxCount;

    public virtual void UesItem()
    {
        
    }
}