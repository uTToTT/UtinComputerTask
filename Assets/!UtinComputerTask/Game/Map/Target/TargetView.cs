using UnityEngine;

public class TargetView : MonoBehaviour, ITargetView
{
    public Vector3 Position => transform.position;
    public Vector3 GroundPosition => new Vector3(transform.position.x, 0, transform.position.z);
}
