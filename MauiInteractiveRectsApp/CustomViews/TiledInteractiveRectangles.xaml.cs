using MauiInteractiveRectsApp.Services;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace MauiInteractiveRectsApp.CustomViews
{
    public partial class TiledInteractiveRectangles : ContentView
    {
        int _movingRectangles = 0;

        Color _baseColor = Colors.Cyan;


        double _tileSize = 50;
        double _marginSize = 10;

        int _rows;
        int _columns;

        TouchTracker _tracker;

        public TiledInteractiveRectangles()
        {
            InitializeComponent(); // need xaml
        }


        async public Task Initialize()
        {
            _tracker = new TouchTracker(myLayout);

            Debug.Print("Initialize View");

            //myLayout.Clear();

            double bodyWidth = this.Width;
            double bodyHeight = this.Height;

            double unitSize = _tileSize + _marginSize;

            _rows = (int)(bodyHeight / unitSize);
            _columns = (int)(bodyWidth / unitSize);

            // for auto padding
            double paddingSizeX = (bodyWidth - unitSize * _columns + _marginSize) / 2;
            double paddingSizeY = (bodyHeight - unitSize * _rows + _marginSize) / 2;

            // rectangle templete
            InteractiveRectangle templete = new InteractiveRectangle(_tileSize, _tileSize);
            templete.SetBaseColor(_baseColor);
            templete.SetTouchTracker(_tracker);

            templete.NotifyMoving += () => _movingRectangles++;
            templete.NotifyStop += () => _movingRectangles--;

            // for test
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    InteractiveRectangle rectangle = templete.Clone();

                    double x = j * unitSize + paddingSizeX;
                    double y = i * unitSize + paddingSizeY;

                    rectangle.SetPosition(x, y);

                    AbsoluteLayout.SetLayoutBounds(rectangle.Instance, new Rect(x, y, _tileSize, _tileSize));
                    myLayout.Add(rectangle.Instance);
                }
            }


            await Task.Delay(1000);
        }


        async public Task NotifyResized()
        {
            await Initialize();
        }

        //
        public int GetTotalRectangles() => _rows * _columns;

        public int GetNumberOfMovingRectangles() => _movingRectangles;

    }
}