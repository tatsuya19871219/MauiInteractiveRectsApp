using System.Diagnostics;

namespace MauiInteractiveRectsApp.Services;

public partial class TouchTracker
{
    View _targetView;

    Point? _position = null;
    Point? _velocity = null;

    Stopwatch _stopwatch = new Stopwatch();

    Point? _lastPosition = null;

    int _callCounts;

    // Delegate functions
    public Action<double?, double?> OnMoved;


    public TouchTracker(View view)
    {
        _targetView = view;

        Initialize();
    }

    public partial void Initialize();


    public Point? GetCurrentPosition() => _position;

    public Point? GetCurrentVelocity() => _velocity;


    private void OnTrackingStart()
    {
        _stopwatch.Start();
        _callCounts = 0;
        _lastPosition = _position;
    }

    private void OnTrackingEnd()
    {
        //_position = null;
        //_velocity = null;
        _stopwatch.Reset();
        _callCounts = 0;
        _lastPosition = null;
    }

    private void OnTracking()
    {
        // 
        if (_stopwatch.Elapsed.TotalSeconds > 1) _stopwatch.Restart();

        double elapsedTime = _stopwatch.Elapsed.TotalSeconds;
        _stopwatch.Restart();

        double vx = (_position.Value.X - _lastPosition.Value.X) / elapsedTime / 1000;
        double vy = (_position.Value.Y - _lastPosition.Value.Y) / elapsedTime / 1000;
        _velocity = new Point(vx, vy);

        _lastPosition = _position;

        _callCounts++;

        if (_callCounts % 100 == 0)
        {
            //Debug.Print($"Elapsed time = {_stopwatch.Elapsed.TotalSeconds} sec per 100 calls");
            //Debug.Print($"Current Velocity = {_velocity}");
        }
    }
}