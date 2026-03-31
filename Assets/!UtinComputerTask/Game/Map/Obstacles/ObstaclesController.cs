using UnityEngine;


// ref to non-MonoBehaviour class
public class ObstaclesController : MonoBehaviour
{
    // add pool
    [SerializeField] private ObstacleView[] _obstacleViews; 
    [SerializeField] private float _power = 2f;
    public void Init()
    {
        for (int i = 0; i < _obstacleViews.Length; i++)
        {
            Obstacle obstacle = new Obstacle(_obstacleViews[i]);
            _obstacleViews[i].Init(obstacle);

            if (i == 7)
            {
                obstacle.Infect(_power);
            }
        }
    }
}
