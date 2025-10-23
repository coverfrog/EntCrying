using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameType _gameType;
    [SerializeField] private List<Game> _gameList = new();

    private Dictionary<GameType, Game> _gameDict;
    
    private void Awake()
    {
        // 게임 목록을 사전화
        _gameDict = _gameList.ToDictionary(g => g.GameType, g => g);
    }

    private void Start()
    {
        // TODO : 외부에서 _gameType 수정하게 만들어서 모드 선택 가능하게
        // 현재는 Basic 모드만 개발된 시점이기에 이 부분은 생략되어 있음
        
        // 게임 목록 중에 선택 된 클래스 선택
        if (!_gameDict.TryGetValue(_gameType, out Game game))
        {
            Debug.Assert(false, "not found game in game list");
            return;
        }

        // 해당 콘텐츠 시작
        game.Begin();
    }
}
