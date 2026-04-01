using UnityEngine;

// Need in refactoring, only for tests
public class ObstaclesController : MonoBehaviour
{
    public const string OBSTACLE_LAYERMASK = "Obstacles";

    [SerializeField] private ObstacleView[] _obstacleViews; 

    public void Init(InfectionController infectionController)
    {
        for (int i = 0; i < _obstacleViews.Length; i++)
        {
            Obstacle obstacle = new Obstacle(_obstacleViews[i], infectionController);
            _obstacleViews[i].Init(obstacle);
        }
    }
}
