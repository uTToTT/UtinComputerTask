using System;
using UnityEngine;

public class PlayerController
{
    public event Action OnOutOfMass;
    public event Action OnReachTarget;

    private const float MIN_MASS = 0.2f;
    private const float CHARGE_SPEED = 1f;
    private const float MOVE_SPEED = 1f;
    private const float PATH_CHECK_RADIUS = 0f;
    private const float WIN_DISTANCE = 0.5f;

    private const float JUMP_DURATION = 0.5f;
    private const float JUMP_HEIGHT = 1f;

    private readonly Player _player;
    private readonly IPlayerView _view;
    private readonly ShotFactory _shotFactory;
    private readonly ShotManager _shotManager;
    private readonly IInputProvider _input;
    private readonly ITargetView _target;

    private bool _isJumping;
    private Vector3 _jumpStart;
    private Vector3 _jumpEnd;
    private float _jumpTime;

    private float _charge;
    private bool _isCharging;

    public PlayerController(
    Player player,
    IPlayerView view,
    IInputProvider input,
    ShotFactory shotFactory,
    ShotManager shotManager,
    ITargetView target)
    {
        _player = player;
        _view = view;
        _input = input;
        _shotFactory = shotFactory;
        _shotManager = shotManager;
        _target = target;
    }

    public void Tick(float dt)
    {
        if (_input.PressedThisFrame)
        {
            StartCharge();
            return;
        }

        if (_input.IsHolding)
        {
            TickCharge(dt);
            return;
        }

        if (_input.ReleasedThisFrame)
        {
            var target = _target.Position;
            ReleaseCharge(target, _player.Position);
            return;
        }

        UpdateMovement(dt);
        CheckWin();
    }

    private void CheckWin()
    {
        float dist = Vector3.Distance(_player.Position, _target.Position);

        if (dist < WIN_DISTANCE)
        {
            OnReachTarget?.Invoke();
        }
    }

    #region Movement

    private void UpdateMovement(float dt)
    {
        if (_isJumping)
        {
            UpdateJump(dt);
            return;
        }

        if (!IsPathClear())
            return;

        StartJump();
    }

    private bool IsPathClear()
    {
        Vector3 direction = (_target.Position - _player.Position).normalized;

        float checkDistance = _player.Radius + 0.5f;

        Vector3 checkPos = _player.Position + direction * checkDistance;

        Collider[] hits = Physics.OverlapSphere(checkPos, _player.Radius + PATH_CHECK_RADIUS);

#if UNITY_EDITOR
        GizmosService.Instance.DrawSphere(
            _player.Position + direction * checkDistance,
            _player.Radius + PATH_CHECK_RADIUS,
            Color.green,
            0.05f);
#endif

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<ObstacleView>(out _))
            {
                return false;
            }
        }

        return true;
    }

    private void MoveTowardsTarget(float dt)
    {
        Vector3 direction = (_target.Position - _player.Position).normalized;

        Vector3 newPos = _player.Position + direction * MOVE_SPEED * dt;

        _player.SetPosition(newPos);
    }

    private void StartJump()
    {
        Vector3 direction = (_target.Position - _player.Position).normalized;

        float stepDistance = _player.Radius * 2f; 

        _jumpStart = _player.Position;
        _jumpEnd = _jumpStart + direction * stepDistance;

        _jumpTime = 0f;
        _isJumping = true;
    }

    private void UpdateJump(float dt)
    {
        _jumpTime += dt;

        float t = _jumpTime / JUMP_DURATION;

        if (t >= 1f)
        {
            _player.SetPosition(_jumpEnd);
            _isJumping = false;
            return;
        }

        Vector3 pos = Vector3.Lerp(_jumpStart, _jumpEnd, t);
        float height = Mathf.Sin(t * Mathf.PI) * JUMP_HEIGHT;

        pos.y += height;

        _player.SetPosition(pos);
    }

    #endregion

    #region Charge / Shoot

    public void StartCharge()
    {
        if (_player.Mass <= MIN_MASS) return;
        _isCharging = true;
        _charge = 0f;
    }

    public void TickCharge(float dt)
    {
        if (!_isCharging) return;

        float chargeDelta = CHARGE_SPEED * dt;

        if (chargeDelta > _player.Mass - MIN_MASS)
            chargeDelta = _player.Mass - MIN_MASS;

        _charge += chargeDelta;
        _player.Mass -= chargeDelta;

        _view.SetScale(_player.Radius); /// ref: scale != mass != radius

        if (_player.Mass <= MIN_MASS)
        {
            _isCharging = false;
            OnOutOfMass?.Invoke();
        }
    }

    public void ReleaseCharge(Vector3 targetPosition, Vector3 playerPosition)
    {
        if (_charge <= 0f) return;

        _isCharging = false;

        var direction = (targetPosition - playerPosition).normalized;

        var shotController = _shotFactory.Create(playerPosition, direction, _charge);
        _shotManager.Add(shotController);

        _charge = 0f;
    }

    #endregion
}
