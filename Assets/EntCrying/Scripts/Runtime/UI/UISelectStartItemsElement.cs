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

    #region OnEnable

    private void OnEnable()
    {
        _disposableSelectedCount = _selectedCount.Subscribe(OnChangeSelectedCount);

        _addButton.onClick.AddListener(OnClickAdd);
        _reduceButton.onClick.AddListener(OnClickReduce);
    }

    #endregion

    #region OnDisable

    private void OnDisable()
    {
        _disposableSelectedCount?.Dispose();
        
        _addButton.onClick.RemoveListener(OnClickAdd);
        _reduceButton.onClick.RemoveListener(OnClickReduce);
    }

    #endregion

    #region Initialize

    public void Initialize(Item item, OnClickCount onClickCount)
    {
        OnActClickCount = onClickCount;
        
        _selectedCount.Value = 0;
        
        _displayNameText.text = item.DisplayName;
        _icon.sprite = item.Icon;
        
        _result.value = item; 
    }

    #endregion

    #region OnClickAdd

    private void OnClickAdd()
    {
        _selectedCount.Value += 1;
        
        OnActClickCount?.Invoke(-1);
    }

    #endregion

    #region OnClickReduce

    private void OnClickReduce()
    {
        if (_selectedCount.Value == 0)
        {
            return;
        }
        
        _selectedCount.Value = Mathf.Max(0, _selectedCount.Value - 1);
        
        OnActClickCount?.Invoke(+1);
    }

    #endregion

    #region OnChangeAbleCount

    public void OnChangeAbleCount(bool enableAddButton)
    {
        _addButton.interactable = enableAddButton;
    }


    #endregion

    #region OnChangeSelectedCount

    private void OnChangeSelectedCount(int count)
    {
        _countText.text = count.ToString();
        _reduceButton.interactable = count > 0;

        _result.count = count;
    }

    #endregion

    #region GetResult

    public CountValue<Item> GetResult() => _result;

    #endregion
    
    

}
