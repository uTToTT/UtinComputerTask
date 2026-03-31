using UnityEngine;

public class Player
{
    private readonly IPlayerView _view;

    public float Mass { get; set; }
    public float Radius => Mass;
    public Vector3 Position => _view.Position;

    public Player(
        IPlayerView view,
        float mass)
    {
        _view = view;
        Mass = mass;

        _view.SetScale(Radius);
    }

    public void SetPosition(Vector3 pos)
    {
        _view.SetPosition(pos);
    }
}
