using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig" , menuName = "UtinComputerTask/Configs/Gameplay/Player")]
public class PlayerConfig : ScriptableObject
{
    [Header("Mass")]
    [SerializeField, Range(5f, 15f)] private float _startMass = 10f;
    [SerializeField, Range(0.1f, 2f)] private float _minMass = 0.2f;
    [SerializeField, Range(0.1f, 10f)] private float _chargeSpeed = 1f;

    [Header("Movement")]
    [SerializeField, Range(0.1f, 3f)] private float _jumpDuration = 0.5f;
    [SerializeField, Range(0.1f, 5f)] private float _jumpHeight = 1f;

    public float StartMass => _startMass;
    public float MinMass   => _minMass;
    public float ChargeSpeed => _chargeSpeed;
    public float JumpDuration => _jumpDuration;
    public float JumpHeight => _jumpHeight;
}
