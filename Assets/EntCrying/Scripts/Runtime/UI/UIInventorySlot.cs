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

    [Header("[ DEBUG ]")] 
    [SerializeField] private Item _item;
    
    private IDisposable _disposableCount;
    
    #region OnDisable

    private void OnDisable()
    {
        _disposableCount?.Dispose();
        _item = null;
    }

    #endregion

    #region SetNull

    public void SetNull()
    {
        _iconImage.sprite = null;
        _countText.text = "";
    }

    #endregion
    
    #region Set

    public void Set(InventorySlot slot)
    {
        if (slot.item == _item)
        {
            return;
        }
        
        _iconImage.sprite = slot.item.Icon;
        
        _disposableCount?.Dispose();
        _disposableCount = slot.count.Subscribe(OnCountChanged);
    }
    
    #endregion

    #region OnCountChanged

    private void OnCountChanged(int count)
    {
        _countText.text = count.ToString();
    }

    #endregion
    
}
