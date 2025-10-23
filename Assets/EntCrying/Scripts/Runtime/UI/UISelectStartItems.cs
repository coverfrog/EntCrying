using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UISelectStartItems : MonoBehaviour
{
    [Serializable]
    public class ElementInitializer
    {
        public UISelectStartItemsElement element;
        public Item item;

        public void Initialize()
        {
            element.Initialize(item);   
        }
    }
    
    public delegate void GameStartDelegate(IEnumerable<UISelectStartItemsElement.Report> reports);

    [Header("[ OPTION ]")]
    [SerializeField] private int _initAbleCount = 3;
    [SerializeField] private List<ElementInitializer> _elementInitializerList = new();
    
    [Header("[ REFERENCE ]")]
    [SerializeField] private TMP_Text _remainAbleCountText;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _blockObject;
    
    private ReactiveProperty<int> _ableCount = new();

    private IDisposable _disposableAbleCount;
    
    private event GameStartDelegate OnActGameStart;

    private List<UISelectStartItemsElement> _elementList = new();

    private void Awake()
    {
        _elementList = _elementInitializerList.Select(x => x.element).ToList();
    }

    private void OnEnable()
    {
        _disposableAbleCount = _ableCount.Subscribe(OnChangeAbleSelectCount);
        
        _gameStartButton.onClick.AddListener(OnGameStart);
        _backButton.onClick.AddListener(OnBack);
        
        foreach (UISelectStartItemsElement element in _elementList)
        {
            element.OnActClickCount += OnActClickCount;
        }
        
        _blockObject.SetActive(false);
        
    }

    private void OnDisable()
    {
        _disposableAbleCount?.Dispose();
        
        _gameStartButton.onClick.RemoveListener(OnGameStart);
        _backButton.onClick.RemoveListener(OnBack);
        
        foreach (UISelectStartItemsElement element in _elementList)
        {
            element.OnActClickCount -= OnActClickCount;
        }
    }
    
    public void Begin(GameStartDelegate onGameStart)
    {
        OnActGameStart -= onGameStart;
        OnActGameStart += onGameStart;
        
        _ableCount.Value = _initAbleCount;
        
        foreach (ElementInitializer initializer in _elementInitializerList)
        {
            initializer.Initialize();
        }
        
        gameObject.SetActive(true);
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
    
    private void OnActClickCount(int add)
    {
        _ableCount.Value += add;
    }

    private void OnChangeAbleSelectCount(int count)
    {
        _remainAbleCountText.text = count.ToString();
        
        _gameStartButton.interactable = count == 0;

        foreach (UISelectStartItemsElement element in _elementList)
        {
            element.OnChangeAbleCount(count > 0);
        }
    }

    private void OnGameStart()
    {
        _blockObject.SetActive(true);
        
        IEnumerable<UISelectStartItemsElement.Report> reports = _elementList.Select(x => x.GetReport());
        
        OnActGameStart?.Invoke(reports);
    }
    
    private void OnBack()
    {
        _blockObject.SetActive(true);
    }
}
