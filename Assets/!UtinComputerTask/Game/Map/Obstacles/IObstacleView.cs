using UnityEngine;

public interface IObstacleView
{
    Vector3 Position { get; }

    void SetColor(Color color);
    void SetColliderEnabled(bool state);
    bool TryGetInfectable(out IInfectable infectable);
}
