using UnityEngine;

public class TargetView : MonoBehaviour, ITargetView
{
    public Vector3 Position => transform.position;
}
