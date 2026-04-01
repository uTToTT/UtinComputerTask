using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoop : IDisposable
{
    public event Action OnStopGame;
    public event Action OnStartGame;
    public event Action OnVictory;
    public event Action OnDefeat;

    private bool _isGameStarted = false;
    private Button _restart;

    public bool IsGameStarted => _isGameStarted;

    //ref: replace UI logic. This approach only for tests
    public GameLoop(Button restart)
    {
        _restart = restart;
        _restart.onClick.AddListener(Restart);
    }

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

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void Dispose()
    {
        _restart.onClick.RemoveAllListeners();
    }
}
