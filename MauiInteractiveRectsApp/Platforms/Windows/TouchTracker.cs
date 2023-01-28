using System.Diagnostics;

namespace MauiInteractiveRectsApp.Services
{
    public partial class TouchTracker
    {

        public partial void Initialize()
        {

            PointerGestureRecognizer gestureRecognizer = new PointerGestureRecognizer();

            gestureRecognizer.PointerEntered += (s, e) =>
            {
                //Debug.Print("Entered");
                _position = e.GetPosition(_targetView);
                OnTrackingStart();
            };

            gestureRecognizer.PointerExited += (s, e) =>
            {
                //Debug.Print("Exited");
                OnTrackingEnd();
            };

            gestureRecognizer.PointerMoved += (s, e) =>
            {
                _position = e.GetPosition(_targetView);
                OnTracking();
            };
            gestureRecognizer.PointerMoved += (s, e) => OnMoved?.Invoke(_position?.X, _position?.Y);

            _targetView.GestureRecognizers.Add(gestureRecognizer);

        }


    }
}