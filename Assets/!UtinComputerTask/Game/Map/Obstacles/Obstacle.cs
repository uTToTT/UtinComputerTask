using System.Threading.Tasks;
using UnityEngine;

public class Obstacle : IInfectable
{
    private const float INFECTION_DIVIDER_POWER = 0.8f;

    private readonly IObstacleView _view;

    private bool _infected;

    public Obstacle(IObstacleView view)
    {
        _view = view;
    }

    public void Infect(float power)
    {
        if (_infected) return;

        _infected = true;

        _view.SetColor(Color.red); /// ref
        _view.SetColliderEnabled(false);

        _ = SpreadInfection(power);
    }

    // ref: add InfectionController
    public async Task SpreadInfection(float power)
    {
        Debug.Log("Spread");
        await Awaitable.WaitForSecondsAsync(0.5f);

        float radius = power;

        // ref to OverlapSphereNonAlloc
        Collider[] hits = Physics.OverlapSphere(_view.Position, radius);

#if UNITY_EDITOR  
        GizmosService.Instance.DrawSphere(_view.Position, radius, Color.yellow, 0.5f);
#endif

        foreach (var hit in hits)
        {
            // for flexibility should use InfactableMap with TryGet(Collider collider, IInfectable)
            if (hit.TryGetComponent<IObstacleView>(out var view)) 
            {
                if (view.TryGetInfectable(out var infectable))
                {
                    infectable.Infect(power * INFECTION_DIVIDER_POWER);
                }
            }
        }
    }
}
