using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIInventorySlot : MonoBehaviour
{
    [Header("[ REFERENCE ]")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;

    private Item _item;
    private IDisposable _disposableCount;

    private InventorySlot _slot;

    private void OnDisable()
    {
        _disposableCount?.Dispose();
    }

    public void Set(InventorySlot slot)
    {
        if (slot == null)
        {
            _slot = null;
            
            _disposableCount?.Dispose();
            
            _iconImage.sprite = null;
            _countText.text = "";
            
            return;
        }

        if (slot == _slot)
        {
            return;
        }
        
        _slot = slot;

        if (_disposableCount == null) _disposableCount = _slot.count.Subscribe(OnCountChanged);
        
        _iconImage.sprite = _slot.item.Icon;
       // _countText.text = _slot.count.ToString();
    }
    
    private void OnCountChanged(int count)
    {
        _countText.text = count.ToString();
    }
}
