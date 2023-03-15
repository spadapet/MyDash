﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using MyDash.Data;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;

    public App()
    {
        this.InitializeComponent();
        this.UpdateMainPage();
    }

    private AppModel model;
    public AppModel Model
    {
        get => this.model;
        set
        {
            if (this.model != value)
            {
                if (this.model != null)
                {
                    this.model.AdoModel.PropertyChanged -= this.OnModelPropertyChanged;
                    this.model.PropertyChanged -= this.OnModelPropertyChanged;
                    this.model.Dispose();
                }

                this.model = value;

                if (this.model != null)
                {
                    this.model.PropertyChanged += this.OnModelPropertyChanged;
                    this.model.AdoModel.PropertyChanged += this.OnModelPropertyChanged;
                }
            }
        }
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Debug.Assert(this.Windows.Count == 0);

        Window window = base.CreateWindow(activationState);
        window.Title = DataResource.AppName;
        window.Stopped += this.OnWindowStopped;

        return window;
    }

    private void OnWindowStopped(object sender, EventArgs args)
    {
        Window window = (Window)sender;
        window.Stopped -= this.OnWindowStopped;

        // Check if the last window is being closed
        if (this.Windows.Count <= 1)
        {
            try
            {
                using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(4)))
                {
                    this.SaveStateAsync(cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
                }
            }
            catch
            {
                // No big deal if settings can't be saved
            }
            finally
            {
                this.Model = null;
            }
        }
    }

    private async Task SaveStateAsync(CancellationToken cancellationToken)
    {
        string json = this.Model.Serialize();
        await File.WriteAllTextAsync(FileUtility.AppModelFile, json, cancellationToken);
    }

    public async Task LoadStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            string json = await File.ReadAllTextAsync(FileUtility.AppModelFile, cancellationToken);
            this.Model = AppModel.Deserialize(json);
        }
        catch
        {
            this.Model = new AppModel();
        }
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        bool all = string.IsNullOrEmpty(args.PropertyName);

        if (this.Model == sender)
        {
            if (all || args.PropertyName == nameof(this.Model.State))
            {
                this.UpdateMainPage();
            }
        }
        else if (this.Model.AdoModel == sender)
        {
            if (all || args.PropertyName == nameof(this.Model.AdoModel.CurrentAccount))
            {
                if (this.MainPage is IUpdatable updatable)
                {
                    updatable.StartUpdate();
                }
            }
        }
    }

    private void UpdateMainPage()
    {
        void EnsureMainPage<T>() where T : Page, new()
        {
            if (this.MainPage is not T)
            {
                this.MainPage = new T();
            }
        }

        switch (this.Model?.State ?? AppState.Loading)
        {
            case AppState.Loading:
                EnsureMainPage<LoadingPage>();
                break;

            case AppState.Login:
                EnsureMainPage<LoginPage>();
                break;

            default:
                EnsureMainPage<MainPage>();
                break;
        }
    }
}
