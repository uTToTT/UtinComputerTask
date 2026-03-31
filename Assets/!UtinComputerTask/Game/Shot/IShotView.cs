using UnityEngine;

public interface IShotView
{
    Vector3 Position { get; }
    void SetScale(float scale);
    void SetPosition(Vector3 pos);
    void SetActive(bool state);
}
