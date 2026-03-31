using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    [SerializeField] private ObstaclesController obstaclesController;

    private void Awake()
    {
        new GameBootstrap().Init();

        obstaclesController.Init();
    }
}
