using UnityEngine;
using DG.Tweening;

public class CameraShaker
{
    private readonly Transform _cameraTransform;

    private Vector3 _initialLocalPos;

    private Tween _shakeTween;

    public CameraShaker(Camera camera)
    {
        _cameraTransform = camera.transform;
        _initialLocalPos = _cameraTransform.localPosition;
    }

    public void Shake(float power)
    {
        float strength = Mathf.Clamp(power, 0.1f, 3f);

        if (_shakeTween != null && _shakeTween.IsActive())
        {
            _shakeTween.Kill();
            _cameraTransform.localPosition = _initialLocalPos;
        }

        _shakeTween = _cameraTransform.DOShakePosition(
                duration: 0.25f,
                strength: strength,
                vibrato: 20,
                randomness: 90f,
                fadeOut: true
            )
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _cameraTransform.localPosition = _initialLocalPos;
            });

        _shakeTween.Play();
    }
}