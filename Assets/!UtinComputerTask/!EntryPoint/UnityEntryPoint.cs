using System;
using UnityEngine;

// Requiered in refactoring: add DI Container, divide on GameBootstrap and installers
public class UnityEntryPoint : MonoBehaviour, IDisposable
{
    [Header("Test")]
    [SerializeField] private ObstaclesController _obstaclesController;

    [Header("Scene context")]
    [SerializeField] private Camera _camera;

    [Header("Scene views")]
    [SerializeField] private TargetView _targetView;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private ShotView _shotPrefab;
    [SerializeField] private PathView _pathView;

    private IInputProvider _inputProvider;

    private PlayerController _playerController;
    private ShotManager _shotManager;
    private GameLoop _gameLoop;

    private void Awake()
    {
        var shotFactory = new ShotFactory(_shotPrefab);
        var player = new Player(_playerView, 5f);

        _shotManager = new ShotManager();
        _inputProvider = new InputProvider(_camera);
        _inputProvider.Enable();
        _gameLoop = new GameLoop();

        _playerController = new PlayerController(
            player,
            _playerView,
            _inputProvider,
            shotFactory,
            _shotManager,
            _targetView,
            _pathView
        );

        _obstaclesController.Init(); /// ref

        _playerController.OnOutOfMass += _gameLoop.Defeat;
        _playerController.OnReachTarget += _gameLoop.Victory;

        _gameLoop.StartGame();
    }

    private void Update()
    {
        if (!_gameLoop.IsGameStarted) return;

        float dt = Time.deltaTime;

        _playerController.Tick(dt);
        _shotManager.Tick(dt);
    }

    private void LateUpdate()
    {
        _inputProvider.LateUpdate();
    }

    public void Dispose()
    {
        _playerController.OnOutOfMass -= _gameLoop.Defeat;
        _playerController.OnReachTarget -= _gameLoop.Victory;
    }
}
