using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Inventory  : IDisposable
{
    public enum ErrorType
    {
        None,
        UnKnown,
        InputCountZero,
        InventoryIsFull,
    }
    
    public class Result
    {
        public bool isSuccess;
        public ErrorType errorType;
    }
    
    public class AddResult : Result
    {
        public int dontAddCount = -1;
        public Item item;
    }
    
    private readonly ReactiveDictionary<int, InventorySlot> _itemDict = new();

    private Dictionary<ReactiveDictEventType, IDisposable> _disposableDict = new();
    
    private UIInventory _ui;

    private const int _slotMaxCount = 15;
    private int _slotCursor;
    
    public void Initialize()
    {
        // UI
        _ui = UIManager.Instance.Inventory;
        _ui?.Initialize();
        
        // 데이터
        _itemDict.Clear();
        
        // 이벤트
        _disposableDict[ReactiveDictEventType.Add] = _itemDict
            .ObserveAdd()
            .Subscribe(x => OnAdd(x.Key, x.Value));
        
        _disposableDict[ReactiveDictEventType.Remove] = _itemDict
            .ObserveRemove()
            .Subscribe(x => OnRemove(x.Key, x.Value));
        
        _disposableDict[ReactiveDictEventType.Replace] = _itemDict
            .ObserveReplace()
            .Subscribe(x => OnReplace(x.Key, x.OldValue, x.NewValue));
    }
    
    public void Dispose()
    {
        foreach (IDisposable disposable in _disposableDict.Values)
        {
            disposable.Dispose();
        }

        _itemDict?.Dispose();
    }
    
    public void ActiveUI(bool active) => _ui?.gameObject.SetActive(active);
    
    public bool TryAddItem(Item item, int count, out AddResult result)
    {
        // 입력 개수 판단
        if (count <= 0)
        {
            result = new AddResult()
            {
                isSuccess = false,
                errorType = ErrorType.InputCountZero,
                dontAddCount = count,
                item = item
            };
            
            return false;
        }
        
        // 기존에 동일한 아이템 있는지 확인
        // * 분할 되어져 있는 아이템이 존재할 가능성이 존재
        foreach ((int idx, InventorySlot slot) in _itemDict.Where(x => x.Value.item == item))
        {
            // 현재 인덱스에 추가 개능한 개수 = 슬롯 전체 개수 - 슬롯 현재 개수
            int ableAddCount = slot.overlapMaxCount - slot.count;
            
            // 현재 인덱스에서 끝남 성공
            if (ableAddCount - count >= 0)
            {
                _itemDict[idx].count += count;
                
                result = new AddResult()
                {
                    isSuccess = true,
                    dontAddCount = 0,
                    item = item
                };
                
                return true;
            }
            
            // 만약에 현재 인덱스에서 전부 더하지 못했다면 더해진 만큼을 제외하고 이월 해야함
            // * 주의 : count 자체도 같이 감소됨
            
            // 감소 시켜야 되는 개수 = Abs(인덱스 추가 가능 개수 - 현재 개수)
            // 예) idx : 1 , slot ( 230, 234 ), add 10
            // Abs((234 - 230 ) - 10)
            // Abs(4 - 10)
            // Abs(-6)
            // 6
            
            int reduceCount = Mathf.Abs(ableAddCount - count);
            count -= reduceCount;
        }

        // 사전의 최대 범위를 넘는지 확인
        // * 내부 아이템 여부 상관없음
        // * 이미 위에서 동일 아이템 있는지 확인 했으므로
        if (_itemDict.Count >= _slotMaxCount)
        {
            result = new AddResult()
            {
                isSuccess = false,
                errorType = ErrorType.InventoryIsFull,
                dontAddCount = count,
                item = item
            };
            
            return false;
        }
        
        // 빈 공간을 찾아서 아이템 추가
        // * 임시 커서를 제공해서 마지막에 추가한 위치를 먼저 탐색
        // * 새롭게 추가되기 때문에 해당 칸의 최대 슬롯값도 재설정
        for (int i = _slotCursor; i < _slotMaxCount; i++)
        {
            bool isAdd = _itemDict.TryAdd(i, new InventorySlot()
            {
                item = item,
                count = count,
                overlapMaxCount = item.SlotOverlapMaxCount
            });

            if (!isAdd)
            {
                continue;
            }

            _slotCursor = i;
            
            result = new AddResult()
            {
                isSuccess = true,
                dontAddCount = 0,
                item = item
            };
            
            return true;
        }

        for (int i = 0; i < _slotMaxCount - _slotCursor; i++)
        {
            bool isAdd = _itemDict.TryAdd(i, new InventorySlot()
            {
                item = item,
                count = count,
                overlapMaxCount = item.SlotOverlapMaxCount
            });

            if (!isAdd)
            {
                continue;
            }
            
            _slotCursor = i;
            
            result = new AddResult()
            {
                isSuccess = true,
                dontAddCount = 0,
                item = item
            };
            
            return true;
        }
        
        // 그 어떠한 행위를 하여도 추가 되지 못했다면 실패
        
        result = new AddResult()
        {
            isSuccess = true,
            errorType = ErrorType.UnKnown,
            dontAddCount = 0,
            item = item
        };
        
        return false;
    }
    
    private void OnAdd(int idx, InventorySlot slot)
    {
#if UNITY_EDITOR && true
        Debug.Log($"[INVENTORY] add at {idx}, {slot.item.CodeName} ( {slot.count} / {slot.overlapMaxCount} )");
#endif
        
        _ui?.OnAdd(idx, slot);
    }
    
    private void OnRemove(int idx, InventorySlot slot)
    {
#if UNITY_EDITOR && true
        Debug.Log($"[INVENTORY] remove at {idx}, {slot.item.CodeName} ( {slot.count} / {slot.overlapMaxCount} )");
#endif
        
        //_ui?.OnRegister(item, count);
    }


    private void OnReplace(int idx, InventorySlot oldSlot, InventorySlot newSlot)
    {
#if UNITY_EDITOR && true
        Debug.Log($"[INVENTORY] replate at {idx} ( {oldSlot.count} / {oldSlot.overlapMaxCount} ) --> ( {newSlot.count} / {newSlot.overlapMaxCount} )");
#endif
       // _ui?.OnReplace(item, oldCount, newCount);
    }
}