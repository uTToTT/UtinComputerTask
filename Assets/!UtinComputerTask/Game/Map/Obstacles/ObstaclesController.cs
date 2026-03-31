using UnityEngine;

// Need in refactoring, only for tests
public class ObstaclesController : MonoBehaviour
{
    [SerializeField] private ObstacleView[] _obstacleViews; 

    public void Init()
    {
        for (int i = 0; i < _obstacleViews.Length; i++)
        {
            Obstacle obstacle = new Obstacle(_obstacleViews[i]);
            _obstacleViews[i].Init(obstacle);
        }
    }
}
