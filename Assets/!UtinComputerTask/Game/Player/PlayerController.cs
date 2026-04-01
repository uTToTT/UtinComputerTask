using System;
using UnityEngine;

public class PlayerController
{
    public event Action OnOutOfMass;
    public event Action OnReachTarget;


    /// <summary>
    /// ref: use data-driven approach based on ScriptableObject
    /// </summary>
    private const float MIN_MASS = 0.2f;
    private const float CHARGE_SPEED = 1f;
    private const float WIN_DISTANCE = 0.5f;

    private const float JUMP_DURATION = 0.5f;
    private const float JUMP_HEIGHT = 1f;
    /// 

    private readonly Player _player;
    private readonly IPlayerView _view;
    private readonly ShotFactory _shotFactory;
    private readonly ShotManager _shotManager;
    private readonly IInputProvider _input;
    private readonly ITargetView _target;
    private readonly IPathView _pathView;

    private Shot _previewShot;
    private IShotView _previewView;
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
    ITargetView target,
    IPathView pathView)
    {
        _player = player;
        _view = view;
        _input = input;
        _shotFactory = shotFactory;
        _shotManager = shotManager;
        _target = target;
        _pathView = pathView;
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
            UpdatePath();
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

    private void UpdatePath()
    {
        Vector3 from = _player.GroundPosition;
        Vector3 to = _target.GroundPosition;

        float width = _player.Radius * 2;

        _pathView.Draw(from, to, width);
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
        UpdatePath();

        if (_isJumping)
        {
            UpdateJump(dt);
            Debug.Log("Jump");
            return;
        }

        if (!IsPathClear())
            return;

        StartJump();
    }

    private bool IsPathClear()
    {
        Vector3 direction = (_target.Position - _player.Position).normalized;
        float distance = _player.Radius * 4f;
        bool result = true;

        if (Physics.SphereCast(
            _player.Position,
            _player.Radius,
            direction,
            out var hit,
            _player.Radius * 2f,
            LayerMask.GetMask(ObstaclesController.OBSTACLE_LAYERMASK)))
        {
            if (hit.collider.TryGetComponent<ObstacleView>(out _))
                result = false;
        }

#if UNITY_EDITOR
        GizmosService.Instance.DrawSphereCast(
            _player.Position,
            direction,
            distance,
            _player.Radius,
            Color.yellow,
            0.1f);
#endif

        return result;
    }

    private void StartJump()
    {
        float distance = Vector3.Distance(_target.Position, _player.Position);

        Vector3 direction = (_target.Position - _player.Position).normalized;

        float stepDistance =Mathf.Min( _player.Radius * 2f,distance);

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

        _previewView = _shotFactory.CreatePreview(_player.Position);
        _previewShot = new Shot(_previewView, 0f);
    }

    public void TickCharge(float dt)
    {
        if (!_isCharging) return;

        float chargeDelta = CHARGE_SPEED * dt;

        if (chargeDelta > _player.Mass - MIN_MASS)
            chargeDelta = _player.Mass - MIN_MASS;

        _charge += chargeDelta;
        _player.Mass -= chargeDelta;

        _view.SetScale(_player.Radius * 2); /// ref: scale != mass != radius
        _previewShot.AddMass(chargeDelta);
        Vector3 direction = (_target.Position - _player.Position).normalized;

        _previewView.SetPosition(
            _player.Position + direction * (_player.Radius + _previewShot.Radius)
        );

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
        var controller = new ShotController(_previewShot, _previewView, direction, 10f);
        _shotManager.Add(controller);

        _charge = 0f;
    }

    #endregion
}
