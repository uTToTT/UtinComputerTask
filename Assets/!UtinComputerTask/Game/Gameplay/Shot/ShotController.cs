using System;
using UnityEngine;

public class ShotController
{
    public event Action<ShotController> OnDead;

    private const float MAX_LIFE_TIME = 5f;

    private readonly IShotView _view;
    private readonly Shot _shot;
    private readonly Vector3 _direction;
    private readonly float _speed;

    private bool _isActive = true;
    private float _lifeTime;

    public bool IsAlive => _isActive;

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
        if (_lifeTime >= MAX_LIFE_TIME) Explode();
        _lifeTime += dt;

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

        /// ref: use OverlapSphereNonAlloc()
        Collider[] hits = Physics.OverlapSphere(_shot.Position, radius);

        float minSqrDistance = float.MaxValue;
        IInfectable closest = null;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<ObstacleView>(out var view))
                continue;

            if (!view.TryGetInfectable(out var infectable))
                continue;

            float sqrDistance = (hit.transform.position - _shot.Position).sqrMagnitude;

            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closest = infectable;
            }
        }

        if (closest != null)
        {
            closest.Infect(radius * 1.5f);
            Explode();
        }
    }

    private void Explode()
    {
        if (!_isActive) return;

        _isActive = false;

#if UNITY_EDITOR
        GizmosService.Instance.DrawSphere(_shot.Position, _shot.Radius, Color.red, 0.2f);
#endif

        //_shot.Deactivate();
        _shot.Destroy(); // ref: ObjectPool;
        OnDead?.Invoke(this);
    }
}
