using MauiInteractiveRectsApp.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace MauiInteractiveRectsApp.CustomViews
{
    public class InteractiveRectangle
    {
        Rectangle _rectangle;

        public Rectangle Instance => _rectangle;

        Color _baseColor = Colors.Black;
        Color _focusColor = Colors.Gray;

        double _x;
        double _y;

        // pointer tracker
        bool _focused;
        TouchTracker _tracker;

        public Action NotifyMoving;
        public Action NotifyStop;

        bool _isMoving;
        int _countEvokeMove;

        //
        public InteractiveRectangle(double width, double height)
        {
            _rectangle = new Rectangle { WidthRequest = width, HeightRequest = height };
            _rectangle.Fill = _baseColor;

            _focused = false;

            _isMoving = false;
            _countEvokeMove = 0;

        }

        public void SetPosition(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Rect GetLayoutBounds() => new Rect(_x, _y, _rectangle.WidthRequest, _rectangle.HeightRequest);

        public void SetBaseColor(Color color)
        {
            _baseColor = color;
            _rectangle.Fill = _baseColor;
        }

        public void SetFocusColor(Color color) => _focusColor = color;

        //
        public void SetTouchTracker(TouchTracker tracker)
        {
            _tracker = tracker;

            _tracker.OnMoved += TrackingFunction;
        }

        public InteractiveRectangle Clone()
        {
            InteractiveRectangle clone = new InteractiveRectangle(_rectangle.WidthRequest, _rectangle.HeightRequest);
            clone.SetBaseColor(_baseColor);
            clone.SetFocusColor(_focusColor);
            clone.SetPosition(_x, _y);

            // set touch tracker
            clone.SetTouchTracker(_tracker);

            clone.NotifyMoving += NotifyMoving;
            clone.NotifyStop += NotifyStop;

            return clone;
        }

        //
        bool OnMe(double px, double py)
        {

            // assume rotationX/Y are 0
            double x0 = _x;
            double y0 = _y;
            double x1 = _x + _rectangle.Width;
            double y1 = _y + _rectangle.Height;

            bool InX = x0 < px && px < x1;
            bool InY = y0 < py && py < y1;

            if (InX && InY) return true;
            return false;
        }

        //
        void TrackingFunction(double? gx, double? gy)
        {
            // null check
            if (!gx.HasValue || !gy.HasValue) return;

            double px = gx.Value;
            double py = gy.Value;

            // check if pointer position in this rectangle
            if (OnMe(px, py))
            {

                // check if focusing is just started
                if (!_focused)
                {
                    Debug.Print($"Just focused on {_rectangle.Id}");
                    _focused = true;

                    _ = OnMeActionEvoke();
                }
                else
                {
                    Point r = GetRelativePosition(px, py);

                    var (angle, distance) = ConvertToAngularCoordinate(r);
                }
            }
            else
            {
                if (_focused)
                {
                    Debug.Print($"Just unfocused on {_rectangle.Id}");
                    _focused = false;
                }
            }
        }

        Point GetRelativePosition(Point point)
        {
            return GetRelativePosition(point.X, point.Y);
        }

        (double angle, double distance) ConvertToAngularCoordinate(Point p)
        {
            double angle = double.NaN;

            try
            {
                angle = Math.Atan2(p.Y, p.X) / Math.PI * 180;
            }
            catch
            {
                Debug.Print("failed to calc angle");
            }
            double distance = Math.Sqrt(p.X * p.X + p.Y * p.Y);

            return (angle, distance);
        }

        Point GetRelativePosition(double px, double py)
        {
            double rx = px - _x - _rectangle.Width / 2; // relative x from rect center
            double ry = py - _y - _rectangle.Height / 2; // relative y

            return new Point(rx, ry);
        }

        async Task OnMeActionEvoke()
        {
            //Debug.Print("On Me Action Evoked!");

            if (!_isMoving)
            {
                _isMoving = true;
                NotifyMoving?.Invoke();
                _countEvokeMove = 1;
            }
            else
            {
                // called twice or more
                _countEvokeMove++;
            }

            double targetRotationAngleOnZ = 0;

            while (true)
            {
                // get current position and velocity from tracker and process
                Point p = (Point)_tracker.GetCurrentPosition();
                Point v = (Point)_tracker.GetCurrentVelocity();

                //Debug.Print($"{p} and {v}");

                // convert to relative state
                Point r = GetRelativePosition(p);

                var (angle, distance) = ConvertToAngularCoordinate(r);

                Point p2 = new Point(r.X + v.X * 10, r.Y + v.Y * 10);

                var (angle2, distance2) = ConvertToAngularCoordinate(p2);

                //Debug.Print($"{angle}, {distance}, {angle2}, {distance2}");

                double rotationAngleOnZ = angle2 - angle;

                //Debug.Print($"Rotation angle: {rotationAngleOnZ}");

                if (rotationAngleOnZ > 0) targetRotationAngleOnZ = 360;
                else if (rotationAngleOnZ < 0) targetRotationAngleOnZ = -360;

                //await Task.Delay(100);

                if (rotationAngleOnZ != 0) break;

                await Task.Delay(100);
            }

            await _rectangle.RotateTo(targetRotationAngleOnZ, 500);

            // finalize action
            _rectangle.Rotation = 0;
            _rectangle.Fill = _baseColor;

            _countEvokeMove--;

            if (_countEvokeMove == 0)
            {
                NotifyStop?.Invoke();
                _isMoving = false;
            }
        }

    }
}