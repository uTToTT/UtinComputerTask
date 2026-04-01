using UnityEngine;

public interface IInfectable
{
    Vector3 Position { get; }

    void Infect(float power);
    void Explode();
}
