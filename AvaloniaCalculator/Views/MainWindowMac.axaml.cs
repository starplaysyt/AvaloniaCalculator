using System;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Platform;
using Avalonia.Native.Interop;
using Avalonia.Threading;
using AvaloniaCalculator.ViewModels;
using ReactiveUI;

namespace AvaloniaCalculator.Views;

public partial class MainWindowMac : Window
{
    public MainWindowMacViewModel ViewModel;
    public MainWindowMac()
    {
        InitializeComponent();
        ViewModel = new MainWindowMacViewModel();
        DataContext = ViewModel;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        ViewModel.LastPressedArgs = e;
        ViewModel.HandleKeysCommand.Execute();
    }

    private void NumericButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.NumericId = Convert.ToInt32((sender as Button).Content as string);;
        ViewModel.NumericButtonClickCommand.Execute();
    }

    private void WindowBase_OnResized(object? sender, WindowResizedEventArgs e)
    {
        Console.WriteLine(ClientSize);
    }
}