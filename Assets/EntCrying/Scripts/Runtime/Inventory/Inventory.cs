using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly ReactiveDictionary<int, InventorySlot> _itemDict = new();

    private Dictionary<ReactiveDictEventType, IDisposable> _disposableDict = new()
    {
        { ReactiveDictEventType.Add     , null },
        { ReactiveDictEventType.Remove  , null },
    };
    
    [Header("[ OPTION ]")]
    [SerializeField] private int _slotMaxCount = 15;
    
    private int _emptyCursor;

    private UIInventory _ui;
    
    #region OnDisable

    public void OnDisable()
    {
        foreach (IDisposable disposable in _disposableDict.Values)
        {
            disposable?.Dispose();
        }
    }

    #endregion
    
    #region Begin

    public void Begin(IReadOnlyDictionary<Item, int> initItemDict)
    {
        // 이벤트 등록 전 아이템 사전 초기화
        _itemDict.Clear();
        
        // 이벤트
        _disposableDict[ReactiveDictEventType.Add]?.Dispose();
        _disposableDict[ReactiveDictEventType.Add] = _itemDict
            .ObserveAdd()
            .Subscribe(x => OnAdd(x.Key, x.Value));
        
        _disposableDict[ReactiveDictEventType.Remove]?.Dispose();
        _disposableDict[ReactiveDictEventType.Remove] = _itemDict
            .ObserveRemove()
            .Subscribe(x => OnRemove(x.Key, x.Value));
 
        // UI
        // * 순서 지킬 것 ( 아이템 등록보단 앞에 있어야함 )
        // * UI 켜지면서 UI 일괄 초기화
        _ui = UIManager.Instance.Inventory;
        _ui.Begin(_slotMaxCount);
        
        // 최초 아이템 등록
        // 없으면 패스
        if (initItemDict == null)
        {
            return;
        }
        
        foreach ((Item item, int count) in initItemDict)
        {
            for (int i = 0; i < count; i++)
            {
                _ = AddItem(item);
            }
        }
    }

    #endregion

    #region AddItem

    public bool AddItem(Item item)
    {
        // 이미 존재하는 아이템 중, 현재 개수 < 최대 슬롯 개수
        List<KeyValuePair<int, InventorySlot>> targets = _itemDict
            .Where(x => (x.Value.item == item) && (x.Value.count.Value < x.Value.item.SlotOverlapMaxCount))
            .ToList();

        // 1개라도 존재한다면
        if (targets.Count > 0)
        {
            // 1번 째 인자 기준
            KeyValuePair<int, InventorySlot> target = targets[0];
            int idx = target.Key;
            
            _itemDict[idx].count.Value += 1;
                

            
            return true;
        }
        
        // 빈 공간 탐색
        int emptyIndex = FindEmpty();
        
        // 없으면 추가 실패
        if (emptyIndex == -1)
        {
            return false;
        }
        
        // 추가
        // * 선언과 값 정의 따로 하는 이유는 이벤트 때문
        _itemDict[emptyIndex] = new InventorySlot(emptyIndex, item);
        _itemDict[emptyIndex].count.Value += 1;
        
        return true;
    }
    
    #endregion

    #region FindEmpty

    private int FindEmpty()
    {
        for (int i = _emptyCursor; i < _slotMaxCount; i++)
        {
            if (_itemDict.ContainsKey(i))
            {
                continue;
            }

            _emptyCursor = i;

            return i;
        }

        for (int i = 0; i < _slotMaxCount - _emptyCursor; i++)
        {
            if (_itemDict.ContainsKey(i))
            {
                continue;
            }
            
            _emptyCursor = i;
            
            return i;
        }

        return -1;
    }

    #endregion
    
    #region Dictionary Events

    private void OnAdd(int idx, InventorySlot slot)
    {
#if UNITY_EDITOR && false
        Debug.Log($"[INVENTORY] add item, index: {idx}, item: {slot.item.CodeName}");
#endif
        
        _ui?.OnAddItem(idx, slot);
    }
    
    private void OnRemove(int idx, InventorySlot slot)
    {
#if UNITY_EDITOR && false
        Debug.Log($"[INVENTORY] remove item, index: {idx}, item: {slot.item.CodeName}");
#endif
        
        //_ui?.OnRegister(item, count);
    }

    #endregion
}