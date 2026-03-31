using UnityEngine;

public class Player
{
    private readonly IPlayerView _view;

    public float Mass { get; set; }
    public float Radius => Mathf.Sqrt(Mass);
    public Vector3 Position => _view.Position;
    public Vector3 GroundPosition => _view.GroundPosition;

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
