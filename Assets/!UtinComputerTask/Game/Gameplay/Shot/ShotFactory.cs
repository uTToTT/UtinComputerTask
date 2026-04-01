using UnityEngine;


// ref: change to ScriptableObject, add objecpool
public class ShotFactory
{
    private readonly ShotView _prefab;

    public ShotFactory(ShotView prefab)
    {
        _prefab = prefab;
    }

    public ShotController Create(Vector3 position, Vector3 direction, float mass)
    {
        var view = Object.Instantiate(_prefab, position, Quaternion.identity);

        var shot = new Shot(view, mass);
        var controller = new ShotController(shot, view, direction, 10f);

        return controller;
    }

    public IShotView CreatePreview(Vector3 position)
    {
        var view = Object.Instantiate(_prefab, position, Quaternion.identity);

        view.SetActive(true);

        return view;
    }
}