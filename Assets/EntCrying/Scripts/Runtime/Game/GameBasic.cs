using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;

public class GameBasic : Game
{
    public override GameType GameType => GameType.Basic;

    [Header("[ REFERENCE ]")]
    [SerializeField] private Inventory _inventory;
    
    [Header("[ DEBUG ]")] 
    [SerializeField] private GameBasicPlayer _player;
    
    private UISelectStartItems _uiSelectStartItems;

    #region Begin

    public override void Begin()
    {
        base.Begin();

        // 플레이어 비 활성화
        _player.End();
        
        // 시작 재화 선택 UI 열기
        _uiSelectStartItems = UIManager.Instance.SelectStartItems;
        _uiSelectStartItems.Begin(OnGameStart, OnGameBack);
    }
    
    #endregion

    #region OnGameStart

    private void OnGameStart(IReadOnlyDictionary<Item, int> initItemDict)
    {
        // 시작 재화 선택 UI 닫기
        _uiSelectStartItems.End();
  
        // 인벤토리 시작
        // * 부모에서 실행하지 않는 이유 : 반드시 인벤토리가 필요하진 않을 것이기에
        _inventory.Begin(initItemDict);
        
        // 플레이어를 생성하고 추가
        // * 현재로썬 단일 플레이어 1개만 추가있으면 된다.
        _player.Begin();
        
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
