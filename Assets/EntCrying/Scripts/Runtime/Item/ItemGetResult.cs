using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class ItemGetResult
{
    private Dictionary<Item, int> _immediateItems = new();
    private Dictionary<Item, int> _addedInventoryItems = new();
    private Dictionary<Item, int> _addFailedInventoryItems = new();

    public IReadOnlyDictionary<Item, int> ImmediateItems => _immediateItems;
    public IReadOnlyDictionary<Item, int> AddedInventoryItems => _addedInventoryItems;
    public IReadOnlyDictionary<Item, int> AddFailedInventoryItems => _addFailedInventoryItems;

    public void Log()
    {
        JObject json = new JObject();
        
        LogDict(ref json, "immediate"         , ref _immediateItems);
        LogDict(ref json, "addedInventory"    , ref _addedInventoryItems);
        LogDict(ref json, "addFailedInventory", ref _addFailedInventoryItems);
        
        Debug.Log(json.ToString(Formatting.Indented));
    }

    private void LogDict(ref JObject json, string key, ref Dictionary<Item, int> dict)
    {
        string rootKey = CUtil.CString.NicifyVariableName(key);

        JArray array = new JArray();
        
        foreach ((Item item, int count) in dict)
        {
            string itemCodeName = CUtil.CString.NicifyVariableName(item.CodeName);
            
            array.Add($"{itemCodeName} : {count}");
        }
        
        json[rootKey] = array;
    }
    
    public void Combine(ItemGetResult source)
    {
        Add_Dict(ref source._immediateItems, ref _immediateItems);
        Add_Dict(ref source._addedInventoryItems, ref _addedInventoryItems);
        Add_Dict(ref source._addFailedInventoryItems, ref _addFailedInventoryItems);
    }
    
    public void Add_Dict(ref Dictionary<Item, int> source, ref Dictionary<Item, int> target)
    {
        foreach ((Item item, int count) in source)
        {
            for (int i = 0; i < count; i++)
            {
                Add_Item(item, ref target);
            }
        }
    }
    
    public void Add_ImmediateItem(Item item)
    {
        Add_Item(item, ref _immediateItems);
    }
    
    public void Add_AddedInventory(Item item)
    {
        Add_Item(item, ref _addedInventoryItems);
    }
    
    public void Add_AddFailedInventory(Item item)
    {
        Add_Item(item, ref _addFailedInventoryItems);
    }

    private void Add_Item(Item item, ref Dictionary<Item, int> dict)
    {
        if (dict.ContainsKey(item))
        {
            dict[item]++;
            return;
        }
        
        dict.Add(item, 1);
    }
}