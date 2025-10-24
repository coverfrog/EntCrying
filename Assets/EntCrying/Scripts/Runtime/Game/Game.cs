using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public abstract GameType GameType { get; }

    protected Inventory _inventory;

    private void OnDisable()
    {
        End();
    }

    public virtual void Begin()
    {
#if UNITY_EDITOR && true
        Debug.Log($"[GAME] begin \"{gameObject.name}\"");
#endif

        _inventory = new Inventory();
    }

    protected virtual void End()
    {
        _inventory.Dispose();
    }

    #region GetItems

    protected ItemGetResult GetItems(IEnumerable<CountValue<Item>> reports)
    {
        ItemGetResult result = new ItemGetResult();

        foreach (CountValue<Item> report in reports)
        {
            Item item = report.value;
            int count = report.count;

            if (count <= 0)
            {
                continue;
            }
            
            ItemGetResult r = GetItem(item, count);
            result.Combine(r);
        }

        return result;
    }

    private ItemGetResult GetItem(Item item, int count)
    {
        // 0 return
        if (count == 0)
        {
            Debug.Assert(false, "count is 0");
        }

        // get
        ItemGetResult result = new ItemGetResult();
        
        for (int i = count - 1; i >= 0; i--)
        {
            GetItem(ref result, item);
        }

        return result;
    }

    private void GetItem(ref ItemGetResult result, Item item)
    {
        // 즉시 사용 아이템 O
        if (item.IsImmediately)
        {
            // 즉시 사용 결과에 추가
            result.Add_ImmediateItem(item);
            item.UesItem();
            
            // 사용 후 사라진다면 그것으로 종료
            if (item.IsDeleteConsumable)
            {
                return;    
            }
        }
        
        // 즉시 사용 아이템 X -> 인벤토리에 추가
        bool isAdd = _inventory.AddItem(item);
        if (isAdd)
            result.Add_AddedInventory(item);
        else
            result.Add_AddFailedInventory(item);
    }
    
    #endregion
}
