using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GateView : MonoBehaviour, IGateView
{
    private const string IS_OPENED = "IsOpened";

    [SerializeField] private Animator _animator;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        _animator.SetBool(IS_OPENED, true);
    }
}
