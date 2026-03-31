using UnityEngine;

// Requiered in refactoring: add DI Container, divide on GameBootstrap and installers
public class UnityEntryPoint : MonoBehaviour
{
    [Header("Test")]
    [SerializeField] private ObstaclesController _obstaclesController;

    [Header("Scene context")]
    [SerializeField] private Camera _camera;

    [Header("Scene views")]
    [SerializeField] private TargetView _targetView;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private ShotView _shotPrefab;

    private IInputProvider _inputProvider;

    private PlayerController _playerController;
    private ShotManager _shotManager;

    private void Awake()
    {
        var shotFactory = new ShotFactory(_shotPrefab);
        var player = new Player(_playerView, 5f);

        _shotManager = new ShotManager();
        _inputProvider = new InputProvider(_camera);

        _playerController = new PlayerController(
            player,
            _playerView,
            _inputProvider,
            shotFactory,
            _shotManager,
            _targetView
        );

        _obstaclesController.Init();

        StartGame();
    }

    private void StartGame()
    {
        _inputProvider.Enable();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _playerController.Tick(dt);
        _shotManager.Tick(dt);
    }

    private void LateUpdate()
    {
        _inputProvider.LateUpdate();
    }
}
