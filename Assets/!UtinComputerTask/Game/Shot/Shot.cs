using UnityEngine;

public class Shot
{
    private readonly IShotView _view;

    public float Mass { get; }
    public float Radius => Mass;

    public Shot(IShotView view, float mass)
    {
        _view = view;
        Mass = mass;

        _view.SetScale(Radius);
        _view.SetActive(true);
    }

    public Vector3 Position => _view.Position;
    public void Deactivate() => _view.SetActive(false);
}
