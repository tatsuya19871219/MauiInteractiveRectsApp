using MauiInteractiveRectsApp.ViewModels;
using Microsoft.Maui.Graphics;
using System.Diagnostics;

namespace MauiInteractiveRectsApp
{
    public partial class MainPage : ContentPage
    {

        AppViewModel _vm;

        public MainPage(AppViewModel vm)
        {
            InitializeComponent();

            BindingContext = _vm = vm;

            StatusLabel.Text = "Initialize..";

            _vm.OnInitialized += () => StatusLabel.Text = "Initialized.";

            _vm.DoSomething += () =>
            {
                StatusLabel.Text = new string('+', _vm.GetNumberOfMovingRectangles());

                FooterBoxView.Color = Colors.LightGreen;
            };


            _ = vm.Initialize(MyTile);

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Debug.Print($"On size allocated (w, h) = ({width}, {height})");

            _vm.ChangeAppSize(width, height);

        }
    }
}