using Android.Views;
using System.Diagnostics;

namespace MauiInteractiveRectsApp.Services
{
    public partial class TouchTracker
    {


        public partial void Initialize()
        {
            MyTouchListener myTouchListener = new MyTouchListener();

            myTouchListener.DownAction += (v, e) =>
            {
                _position = myTouchListener.GetCurrentPosition();
                OnTrackingStart();
            };

            myTouchListener.UpAction += (v, e) =>
            {
                OnTrackingEnd();
            };

            myTouchListener.MoveAction += (v, e) =>
            {
                _position = myTouchListener.GetCurrentPosition();
                OnTracking();
            };

            myTouchListener.MoveAction += (v, e) => OnMoved?.Invoke(_position?.X, _position?.Y);

            var androidTargetView = _targetView.Handler.PlatformView as Android.Views.View;

            androidTargetView.SetOnTouchListener(myTouchListener);
        }


        // listener class
        private class MyTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            public Action<Android.Views.View, Android.Views.MotionEvent> MoveAction;
            public Action<Android.Views.View, Android.Views.MotionEvent> DownAction;
            public Action<Android.Views.View, Android.Views.MotionEvent> UpAction;


            // current point
            Point? _currentPoint;

            // display info
            float _density;

            public MyTouchListener() : base()
            {
                _density = (float)DeviceDisplay.Current.MainDisplayInfo.Density;
            }

            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                //Debug.Print($"Action => {e.Action}");

                switch (e.Action)
                {
                    case Android.Views.MotionEventActions.Down:
                        // touch
                        _currentPoint = ConvertToDIP(e.GetX(), e.GetY());
                        DownAction?.Invoke(v, e);

                        break;

                    case Android.Views.MotionEventActions.Up:
                        // detouch
                        _currentPoint = ConvertToDIP(e.GetX(), e.GetY());
                        UpAction?.Invoke(v, e);
                        _currentPoint = null;
                        break;

                    case Android.Views.MotionEventActions.Move:

                        _currentPoint = ConvertToDIP(e.GetX(), e.GetY());
                        MoveAction?.Invoke(v, e);

                        break;

                    default:
                        // NOP
                        break;
                }

                return true;
            }

            Point ConvertToDIP(double x, double y)
            {
                return new Point(x / _density, y / _density);
            }

            public Point? GetCurrentPosition() => _currentPoint;
        }
    }
}