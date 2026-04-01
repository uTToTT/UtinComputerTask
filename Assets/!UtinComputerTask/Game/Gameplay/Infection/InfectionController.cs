using System.Threading.Tasks;
using UnityEngine;

public class InfectionController
{
    private const float INFECTION_DIVIDER_POWER = 0.8f;
    private const float SPREAD_DELAY = 0.5f;

    private readonly Collider[] _buffer = new Collider[32]; 

    public async Task SpreadAsync(IInfectable sourceView, float power)
    {
        await Awaitable.WaitForSecondsAsync(SPREAD_DELAY);

        float radius = power;

        int hitsCount = Physics.OverlapSphereNonAlloc(
            sourceView.Position,
            radius,
            _buffer
        );

#if UNITY_EDITOR
        GizmosService.Instance.DrawSphere(sourceView.Position, radius, Color.yellow, 0.5f);
#endif

        for (int i = 0; i < hitsCount; i++)
        {
            var hit = _buffer[i];

            if (hit.TryGetComponent<IObstacleView>(out var view))
            {
                if (view.TryGetInfectable(out var infectable))
                {
                    infectable.Infect(power * INFECTION_DIVIDER_POWER);
                }
            }
        }

        await Awaitable.WaitForSecondsAsync(SPREAD_DELAY);
        sourceView.Explode();
    }
}