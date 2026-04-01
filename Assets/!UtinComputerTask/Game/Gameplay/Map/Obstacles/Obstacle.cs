using UnityEngine;

public class Obstacle : IInfectable
{
    private readonly IObstacleView _view;
    private readonly InfectionController _infectionController;

    private bool _infected;

    public Vector3 Position => _view.Position;

    public Obstacle(IObstacleView view, InfectionController infectionController)
    {
        _view = view;
        _infectionController = infectionController;
    }

    public void Infect(float power)
    {
        if (_infected) return;

        _infected = true;

        _view.SetColor(Color.blue); // ref: color to data-driven
        //_view.SetColliderEnabled(false);

        _ = _infectionController.SpreadAsync(this, power);
    }

    public void Explode()
    {
        /// Ref: implement objectPool
        //_view.SetActive(false);
        _view.Destroy();
    }
}