using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

public class GameBasic : Game
{
    public override GameType GameType => GameType.Basic;

    private UISelectStartItems _uiSelectStartItems;
    
    public override void Begin()
    {
        base.Begin();
        
        // 시작 재화 선택 UI 열기
        _uiSelectStartItems = UIManager.Instance.SelectStartItems;
        _uiSelectStartItems.Begin(OnGameStart, OnGameBack);
    }

    private void OnGameStart(IReadOnlyDictionary<Item, int> initItemDict)
    {
        // 시작 재화 선택 UI 닫기
        _uiSelectStartItems.End();
  
        // 인벤토리 시작
        _inventory.Begin(initItemDict);
        
        // 선택에 따른 결과 아이템들을 인벤토리에 추가
        // ItemGetResult result = GetItems(reports);
        
        // 인벤토리 열기

        // result.Log();
    }

    private void OnGameBack()
    {
        Debug.Log("back");
    }

}
