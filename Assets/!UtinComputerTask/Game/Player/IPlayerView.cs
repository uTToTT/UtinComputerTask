using UnityEngine;

public interface IPlayerView
{
    Vector3 Position { get; }
    void SetScale(float scale);
    void SetPosition(Vector3 pos);
}
