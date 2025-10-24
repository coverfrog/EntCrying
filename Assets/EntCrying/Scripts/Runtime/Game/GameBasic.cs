using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

public class GameBasic : Game
{
    public override GameType GameType => GameType.Basic;

    [Header("[ OPTION ]")] 
    [SerializeField] private List<Money> _monies = new();
    
    [Header("[ REFERENCE ]")]
    [SerializeField] private Inventory _inventory;
    
    #region Begin

    public override void Begin()
    {
        base.Begin();

        // 인벤토리 시작
        _inventory.Begin(null);
        
        // 재화 초기화
        _monies = _monies.Select(m => (Money)m.Clone()).ToList();
        _monies.ForEach(m => m.SetMoneyValue(this, 0));
        
        // 재화 UI 연결
        UIManager.Instance.Monies.Begin(_monies);
        
        // 시작 재화 선택 UI 열기
        //UIManager.Instance.SelectStartItems.Begin(OnGameStart, OnGameBack);
    }
    
    #endregion

    #region OnGameStart

    private void OnGameStart(IReadOnlyDictionary<Item, int> initItemDict)
    {
        // 시작 재화 선택 UI 닫기
        UIManager.Instance.SelectStartItems.End();
        
        // TODO : Input 활성화
        // New Input 으로 갈지는 아직 고민
    }
    
    #endregion

    #region OnGameBack

    private void OnGameBack()
    {
        Debug.Log("back");
    }

    #endregion
}
