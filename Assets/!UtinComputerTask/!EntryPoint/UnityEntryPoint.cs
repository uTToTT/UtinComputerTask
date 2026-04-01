using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO: 
/// - DI Container
/// - Divide on GameBootstrap and installers
/// - Implement SctiptableObjects Configs
/// </summary>
public class UnityEntryPoint : MonoBehaviour, IDisposable
{
    [Header("Test")]
    [SerializeField] private ObstaclesController _obstaclesController;
    [SerializeField] private Button _restartButton;
    [SerializeField, Min(1)] private int _targetFPS = 60;

    [Header("Scene context")]
    [SerializeField] private Camera _camera;

    [Header("Scene views")]
    [SerializeField] private TargetView _targetView;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private ShotView _shotPrefab;
    [SerializeField] private PathView _pathView;
    [SerializeField] private GateView _gateView;

    [Header("UI")]
    [SerializeField] private WindowController _windowController;

    [Header("Configs")]
    [SerializeField] private PlayerConfig _playerConfig;

    private IInputProvider _inputProvider;

    private PlayerController _playerController;
    private ShotManager _shotManager;
    private GameLoop _gameLoop;
    private Gate _gate;
    private InfectionController _infectionController;
    private CameraShaker _cameraShaker;

    private void Awake()
    {
        Application.targetFrameRate = _targetFPS;
        QualitySettings.vSyncCount = 0; 

        var shotFactory = new ShotFactory(_shotPrefab);
        var player = new Player(_playerView, _playerConfig);

        _shotManager = new ShotManager();
        _inputProvider = new InputProvider(_camera);
        _inputProvider.Enable();
        _gameLoop = new GameLoop(_restartButton);
        _gate = new Gate(_gateView, _playerView);
        _infectionController = new InfectionController();
        _cameraShaker = new CameraShaker(_camera);
        
        _playerController = new PlayerController(
            player,
            _playerView,
            _inputProvider,
            shotFactory,
            _shotManager,
            _targetView,
            _pathView
        );

        _obstaclesController.Init(_infectionController); /// ref

        _playerController.OnOutOfMass += _gameLoop.Defeat;
        _playerController.OnReachTarget += _gameLoop.Victory;

        _gameLoop.OnVictory += _windowController.EnableWinWindow;
        _gameLoop.OnDefeat += _windowController.EnableLoseWindow;

        _playerController.OnGroundedMass += _cameraShaker.Shake;
        _playerController.OnGroundedMass += _playerView.PlayGrounded;

        _gameLoop.StartGame();
    }

    private void Update()
    {
        if (!_gameLoop.IsGameStarted) return;

        float dt = Time.deltaTime;

        _playerController.Tick(dt);
        _shotManager.Tick(dt);
        _gate.Tick(dt);
    }

    private void LateUpdate()
    {
        _inputProvider.LateUpdate();
    }

    public void Dispose()
    {
        _playerController.OnOutOfMass -= _gameLoop.Defeat;
        _playerController.OnReachTarget -= _gameLoop.Victory;

        _gameLoop.OnVictory -= _windowController.EnableWinWindow;
        _gameLoop.OnDefeat -= _windowController.EnableLoseWindow;

        _playerController.OnGroundedMass -= _cameraShaker.Shake;
        _playerController.OnGroundedMass -= _playerView.PlayGrounded;
    }
}
