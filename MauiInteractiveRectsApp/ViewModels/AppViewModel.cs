using CommunityToolkit.Mvvm.ComponentModel;
using MauiInteractiveRectsApp.CustomViews;
using System.Diagnostics;

namespace MauiInteractiveRectsApp.ViewModels
{
    public partial class AppViewModel : ObservableObject
    {
        TiledInteractiveRectangles _view;

        double _headerProportion = 0.1;
        double _footerProportion = 0.1;

        [ObservableProperty] double appWidth;
        [ObservableProperty] double appHeight;
        [ObservableProperty] double headerHeight;
        [ObservableProperty] double footerHeight;
        [ObservableProperty] double bodyHeight;

        public Action OnInitialized;
        public Action DoSomething;

        async public Task Initialize(TiledInteractiveRectangles view)
        {
            _view = view;

            await WaitFor(() => view.Handler != null);

            await WaitFor(() => AppWidth != 0 && AppHeight != 0);


            await view.Initialize();

            OnInitialized?.Invoke();

            _ = BackgroundProcess();

        }

        async Task BackgroundProcess()
        {
            while (true)
            {
                DoSomething?.Invoke();
                await Task.Delay(100);
            }
        }


        // Wait function
        async Task WaitFor(Func<bool> condition)
        {
            while (true)
            {
                if (condition()) break;
                await Task.Delay(100);
            }
        }

        public void ChangeAppSize(double width, double height)
        {
            AppWidth = width;
            AppHeight = height;
            HeaderHeight = _headerProportion * height;
            FooterHeight = _footerProportion * height;
            BodyHeight = AppHeight - HeaderHeight - FooterHeight;
        }

        // To acquire status 
        public int GetNumberOfMovingRectangles() => _view.GetNumberOfMovingRectangles();

    }
}