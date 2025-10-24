using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIInventory : MonoBehaviour
{
    [Header("[ RESOURCE ]")] 
    [SerializeField] private UIInventorySlot _slotPrefab;

    [Header("[ REFERENCE ]")]
    [SerializeField] private RectTransform _slotContentRt;

    private IObjectPool<UIInventorySlot> _pool;
    private List<UIInventorySlot> _slotList = new List<UIInventorySlot>();

    #region Begin

    public void Begin(int slotMaxCount)
    {
        // Pool 초기화
        if (_pool == null)
        {
            _pool = new ObjectPool<UIInventorySlot>(
                createFunc: () =>
                {
                    UIInventorySlot slot = Instantiate(_slotPrefab, _slotContentRt);
                    _slotList.Add(slot);

                    return slot;
                },
                actionOnGet: slot =>
                {
                    slot.gameObject.SetActive(true);
                },
                actionOnRelease: slot =>
                {
                    slot.gameObject.SetActive(false);
                },
                actionOnDestroy: slot =>
                {
                    Destroy(slot.gameObject);
                });
        }
        
        // 필요 개수
        int addCount = slotMaxCount - _pool.CountInactive;

        for (int i = 0; i < Mathf.Abs(addCount); i++)
        {
            if (addCount > 0)
            {
                _ = _pool.Get();
            }

            else
            {
                _pool.Release(_slotList[_slotList.Count - 1 - i]);
            }
        }
        
        // 슬롯 전부 초기화
        foreach (UIInventorySlot slot in _slotList)
        {
            slot.SetNull();
        }
        
        // 화면 열기
        gameObject.SetActive(true);
    }
    
    #endregion

    #region OnAddItem
    
    public void OnAddItem(int idx, InventorySlot slot)
    {
        // 적용
        _slotList[idx].Set(slot);
    }
    
    #endregion

    #region OnRemoveItem
    
    public void OnRemoveItem(int idx, InventorySlot slot)
    {
        
    }
    
    #endregion
    
}
