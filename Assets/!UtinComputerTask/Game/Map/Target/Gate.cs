using UnityEngine;

public class Gate 
{
    private const float TRIGGER_DISTANCE = 5f;

    private readonly IGateView _view;
    private readonly IPlayerView _player;

    private bool _isOpened = false;

    public Gate(IGateView view, IPlayerView player)
    {
        _view = view;
        _player = player;
    }

    public void Tick(float dt)
    {
        if (_isOpened) return;

        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(_player.Position, _view.Position);

        if (distance <= TRIGGER_DISTANCE + _player.Scale / 2)
        {
            OpenGates();
        }
    }

    private void OpenGates()
    {
        _view.Open();
        _isOpened = true;
    }
}
