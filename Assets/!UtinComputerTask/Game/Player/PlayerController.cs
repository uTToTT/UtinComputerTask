using System;
using UnityEngine;

public class PlayerController
{
    public event Action OnOutOfMass;

    private readonly Player _player;
    private readonly IPlayerView _view;
    private readonly ShotFactory _shotFactory;
    private readonly ShotManager _shotManager;
    private readonly IInputProvider _input;
    private readonly ITargetView _target;

    private float _charge;
    private bool _isCharging;

    private const float MIN_MASS = 0.2f;
    private const float CHARGE_SPEED = 1f;

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
        }

        if (_input.IsHolding)
        {
            TickCharge(dt);
        }

        if (_input.ReleasedThisFrame)
        {
            var target = _target.Position;
            ReleaseCharge(target, _player.Position);
        }
    }

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

        _view.SetScale(_player.Radius);

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
