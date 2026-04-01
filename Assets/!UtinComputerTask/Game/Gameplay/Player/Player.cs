using UnityEngine;

public class Player
{
    private readonly IPlayerView _view;

    public float Mass { get; set; }
    public float Radius => Mathf.Sqrt(Mass);
    public Vector3 Position => _view.Position;
    public Vector3 GroundPosition => _view.GroundPosition;

    private readonly PlayerConfig _config;

    public PlayerConfig Config => _config;

    public Player(
        IPlayerView view,
        PlayerConfig config)
    {
        _config = config;

        _view = view;
        Mass = _config.StartMass;

        _view.SetScale(Radius * 2);
    }

    public void SetPosition(Vector3 pos)
    {
        _view.SetPosition(pos);
    }
}
