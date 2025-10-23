using System;
using System.Collections.Generic;
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
        _uiSelectStartItems.Begin(OnGameStart);
    }

    private void OnGameStart(IEnumerable<UISelectStartItemsElement.Report> reports)
    {
        // 시작 재화 선택 UI 닫기
        _uiSelectStartItems.End();
        
        // 선택에 따른 결과 아이템들을 인벤토리에 추가
        foreach (UISelectStartItemsElement.Report report in reports)
        {
            Item item = report.item;
            int count = report.count;

            if (count <= 0)
            {
                continue;
            }
            
            _ = _inventory.TryAddItem(item, count, out _);
            
        }
        
        // 인벤토리 열기
        _inventory.ActiveUI(true);
    }
}
