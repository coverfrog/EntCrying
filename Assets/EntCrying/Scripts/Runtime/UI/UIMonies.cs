using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIMonies : MonoBehaviour
{
    [Header("[ REFERENCE ]")]
    [SerializeField] private RectTransform _contentRt;

    [Header("[ RESOURCE]")]
    [SerializeField] private UIMoniesElement _elementPrefab;

    private IObjectPool<UIMoniesElement> _pool;
    private List<UIMoniesElement> _elementList = new();
    
    public void Begin(List<Money> monies)
    {
        // Pool 초기화
        if (_pool == null)
        {
            _pool = new ObjectPool<UIMoniesElement>(
                createFunc: () =>
                {
                    UIMoniesElement ele = Instantiate(_elementPrefab, _contentRt);
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
        int initCount = monies.Count;
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
            Money money = monies[i];
            UIMoniesElement element = _elementList[i];
            
            element.Initialize(money);
        }
        
        // Active
        gameObject.SetActive(true);
    }
}