using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UISelectStartItems : MonoBehaviour
{
    public delegate void GameStartDelegate(IReadOnlyDictionary<Item, int> initItemDict);
    public delegate void GameBackDelegate();

    [Header("[ OPTION ]")]
    [SerializeField] private int _initAbleCount = 3;
    [SerializeField] private List<Item> _initItemList = new();
    
    [Header("[ RESOURCE ]")] 
    [SerializeField] private UISelectStartItemsElement _elementPrefab;

    [Header("[ REFERENCE ]")] 
    [SerializeField] private RectTransform _elementContentRt;
    [SerializeField] private TMP_Text _remainAbleCountText;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _blockObject;
    
    private ReactiveProperty<int> _ableCount = new();

    private IDisposable _disposableAbleCount;
    
    private GameStartDelegate _onActGameStart;
    private GameBackDelegate _onActGameBack;

    private IObjectPool<UISelectStartItemsElement> _pool;
    private List<UISelectStartItemsElement> _elementList = new();

    #region OnEnable

    private void OnEnable()
    {
        _disposableAbleCount = _ableCount.Subscribe(OnChangeAbleSelectCount);
        
        _gameStartButton.onClick.AddListener(OnGameStart);
        _backButton.onClick.AddListener(OnGameBack);
        
        _blockObject.SetActive(false);
    }

    #endregion

    #region OnDisable

    private void OnDisable()
    {
        _disposableAbleCount?.Dispose();
        
        _gameStartButton.onClick.RemoveListener(OnGameStart);
        _backButton.onClick.RemoveListener(OnGameBack);
    }

    #endregion

    #region Begin

    public void Begin(GameStartDelegate onGameStart, GameBackDelegate onGameBack)
    {
        // Pool 초기화
        if (_pool == null)
        {
            _pool = new ObjectPool<UISelectStartItemsElement>(
                createFunc: () =>
                {
                    UISelectStartItemsElement ele = Instantiate(_elementPrefab, _elementContentRt);
                    _elementList.Add(ele);

                    return ele;
                },
                actionOnGet: ele =>
                {
                    ele.gameObject.SetActive(true);
                },
                actionOnRelease: ele =>
                {
                    ele.gameObject.SetActive(false);
                },
                actionOnDestroy: ele =>
                {
                    Destroy(ele.gameObject);
                });
        }
        
        // 필요 개수
        int initCount = _initItemList.Count;
        int addCount = initCount - _pool.CountInactive;

        for (int i = 0; i < Mathf.Abs(addCount); i++)
        {
            if (addCount > 0)
            {
                _ = _pool.Get();
            }

            else
            {
                _pool.Release(_elementList[_elementList.Count - 1 - i]);
            }
        }
        
        // 값 부여
        for (int i = 0; i < initCount; i++)
        {
            Item item = _initItemList[i];
            UISelectStartItemsElement element = _elementList[i];
            
            element.Initialize(item, OnActClickCount);
        }
        
        // 이벤트 등록
        _onActGameStart = onGameStart;
        _onActGameBack = onGameBack;
        
        // 값 초기화
        _ableCount.Value = _initAbleCount;
        
        // 켜기
        gameObject.SetActive(true);
    }

    #endregion

    #region End

    public void End()
    {
        gameObject.SetActive(false);
    }
    
    #endregion

    #region OnActClickCount
    
    private void OnActClickCount(int add)
    {
        _ableCount.Value += add;
    }
    
    #endregion

    #region OnChangeAbleSelectCount

    private void OnChangeAbleSelectCount(int count)
    {
        _remainAbleCountText.text = count.ToString();
        
        _gameStartButton.interactable = count == 0;

        foreach (UISelectStartItemsElement element in _elementList)
        {
            element.OnChangeAbleCount(count > 0);
        }
    }

    #endregion

    #region OnGameStart

    private void OnGameStart()
    {
        // 상호작용 막기
        _blockObject.SetActive(true);

        // 초기 아이템 선언
        Dictionary<Item, int> initItemDict = _elementList
            .Select(x => x.GetResult())
            .Where(x => x.count > 0)
            .GroupBy(x => x.value)
            .ToDictionary(x => x.Key, x => x.Sum(y => y.count));
        
        // 게임 시작 이벤트 콜백
        _onActGameStart?.Invoke(initItemDict);
    }
    
    #endregion

    #region OnGameBack

    private void OnGameBack()
    {
        _blockObject.SetActive(true);
        
        _onActGameBack?.Invoke();
    }

    #endregion
    
    
}
