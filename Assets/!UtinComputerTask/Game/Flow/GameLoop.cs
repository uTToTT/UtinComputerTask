using System;
using UnityEngine;

public class GameLoop 
{
    public event Action OnStopGame;
    public event Action OnStartGame;
    public event Action OnVictory;
    public event Action OnDefeat;

    private bool _isGameStarted = false;

    public bool IsGameStarted => _isGameStarted;

    public void Victory()
    {
        Debug.Log("WIN");
        OnVictory?.Invoke();
        StopGame();
    }

    public void Defeat()
    {
        Debug.Log("LOSE");
        OnDefeat?.Invoke();
        StopGame();
    }

    public void StartGame()
    {
        if (_isGameStarted) return;

        _isGameStarted = true;
        OnStartGame?.Invoke();
    }

    private void StopGame()
    {
        if (!_isGameStarted) return;

        _isGameStarted = false;
        OnStopGame?.Invoke();
    }
}
