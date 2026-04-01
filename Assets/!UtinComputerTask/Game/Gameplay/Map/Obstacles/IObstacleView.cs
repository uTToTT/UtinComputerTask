using UnityEngine;

public interface IObstacleView
{
    Vector3 Position { get; }

    void SetColor(Color color);
    void SetColliderEnabled(bool state);
    void SetActive(bool state);
    void Destroy(); /// Only for test, better use objectPool
    bool TryGetInfectable(out IInfectable infectable);
}
