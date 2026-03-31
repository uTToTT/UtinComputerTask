using UnityEngine;

public class ShotController
{
    private readonly IShotView _view;
    private readonly Shot _shot;
    private readonly Vector3 _direction;
    private readonly float _speed;

    private bool _isActive = true;

    public ShotController(
        Shot shot,
        IShotView view,
        Vector3 direction,
        float speed)
    {
        _shot = shot;
        _view = view;
        _direction = direction;
        _speed = speed;
    }

    public void Tick(float dt)
    {
        if (!_isActive) return;

        Move(dt);
        CheckCollision();
    }

    private void Move(float dt)
    {
        var pos = _shot.Position;
        pos += _direction * _speed * dt;

        _view.SetPosition(pos);
    }

    private void CheckCollision()
    {
        float radius = _shot.Radius;

        Collider[] hits = Physics.OverlapSphere(_shot.Position, radius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<ObstacleView>(out var view))
            {
                if (view.TryGetInfectable(out var infectable))
                {
                    infectable.Infect(radius);
                    Explode();
                    return;
                }
            }
        }
    }

    private void Explode()
    {
        _isActive = false;

#if UNITY_EDITOR
        GizmosService.Instance.DrawSphere(_shot.Position, _shot.Radius, Color.red, 0.2f);
#endif

        _shot.Deactivate();
    }
}
