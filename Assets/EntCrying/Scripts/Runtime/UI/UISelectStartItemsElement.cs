using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UISelectStartItemsElement : MonoBehaviour
{
    public delegate void OnClickCount(int add);
    
    [Header("[ REFERENCE ]")]
    [SerializeField] private TMP_Text _displayNameText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _reduceButton;

    [Header("[ DEBUG ]")]
    [SerializeField] private CountValue<Item> _result;
    
    private OnClickCount OnActClickCount;
    
    private ReactiveProperty<int> _selectedCount = new();

    private IDisposable _disposableSelectedCount;

    private void OnEnable()
    {
        _disposableSelectedCount = _selectedCount.Subscribe(OnChangeSelectedCount);

        _addButton.onClick.AddListener(OnClickAdd);
        _reduceButton.onClick.AddListener(OnClickReduce);
    }

    private void OnDisable()
    {
        _disposableSelectedCount?.Dispose();
        
        _addButton.onClick.RemoveListener(OnClickAdd);
        _reduceButton.onClick.RemoveListener(OnClickReduce);
    }

    public void Initialize(Item item, OnClickCount onClickCount)
    {
        OnActClickCount = onClickCount;
        
        _selectedCount.Value = 0;
        
        _displayNameText.text = item.DisplayName;
        _icon.sprite = item.Icon;
        
        _result.value = item; 
    }

    private void OnClickAdd()
    {
        _selectedCount.Value += 1;
        
        OnActClickCount?.Invoke(-1);
    }

    private void OnClickReduce()
    {
        if (_selectedCount.Value == 0)
        {
            return;
        }
        
        _selectedCount.Value = Mathf.Max(0, _selectedCount.Value - 1);
        
        OnActClickCount?.Invoke(+1);
    }

    public void OnChangeAbleCount(bool enableAddButton)
    {
        _addButton.interactable = enableAddButton;
    }
    
    private void OnChangeSelectedCount(int count)
    {
        _countText.text = count.ToString();
        _reduceButton.interactable = count > 0;

        _result.count = count;
    }

    public CountValue<Item> GetResult() => _result;
}
