# MauiInteractiveRectsApp

An application that rotates each square tile by tracing the shape with touch (Android) or mouse hover (Windows).

## Files edited
 - CustomViews/
     - [InteractiveRectangle.cs](./MauiInteractiveRectsApp/CustomViews/InteractiveRectangle.cs)
     - [TiledInteractiveRectangles.xaml](./MauiInteractiveRectsApp/CustomViews/TiledInteractiveRectangles.xaml)
     - [TiledInteractiveRectangles.xaml.cs](./MauiInteractiveRectsApp/CustomViews/TiledInteractiveRectangles.xaml.cs)
 - Platforms/
     - Android/
         - [TouchTracker.cs](./MauiInteractiveRectsApp/Platforms/Android/TouchTracker.cs)
     - Windows/
         - [TouchTracker.cs](./MauiInteractiveRectsApp/Platforms/Windows/TouchTracker.cs)
 - Services/
     - [TouchTracker.cs](./MauiInteractiveRectsApp/Services/TouchTracker.cs)
 - ViewModels/
     - [AppViewModel.cs](./MauiInteractiveRectsApp/ViewModels/AppViewModel.cs)
 - [MainPage.xaml](./MauiInteractiveRectsApp/MainPage.xaml)
 - [MainPage.xaml.cs](./MauiInteractiveRectsApp/MainPage.xaml.cs)
 - [MauiProgram.cs](./MauiInteractiveRectsApp/MauiProgram.cs)

## What I learnt from this project

- How to manipulate shapes (Translation, Rotation, etc.) 
- How to create and use custom views that inherit from ContentView
- How to create platform-specific classes
- How to use GestureRecognizer
- How to pause debugging on exception in Visual Studio

### Platform specific codes

Partial class can be used to introduce platform-specific functions. In this project, I added TouchTracker class for detecting touch position (Android) or pointer position (Windows) because MAUI GestureRecognizer can't detect global touch position in Android. The platform-specific codes are located in the corresponding subdirectory under Platform.

Common part
```csharp
namespace MauiInteractiveRectsApp.Services;

public partial class TouchTracker
{
    View _targetView;

    Point? _position = null;
    Point? _velocity = null;

    public TouchTracker(View view)
    {
        _targetView = view;

        Initialize();
    }

    public partial void Initialize();
}
```

Windows-specific part
```csharp
namespace MauiInteractiveRectsApp.Services;

public partial class TouchTracker
{

    public partial void Initialize()
    {
        ... // Windows-specific codes
    }

}
```

Android-specific part
```csharp
namespace MauiInteractiveRectsApp.Services;

public partial class TouchTracker
{

    public partial void Initialize()
    {
        ... // Android-specific codes
    }

}
```
