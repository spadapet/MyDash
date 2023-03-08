﻿using System;

namespace MyDash.Data;

/// <summary>
/// Represents some ongoing work
/// </summary>
public sealed class WorkData : PropertyNotifier
{
    private readonly Action cancelAction;

    public WorkData(string text, Action cancelAction = null)
    {
        this.text = text ?? string.Empty;
        this.cancelAction = cancelAction;
    }

    private string text;
    public string Text
    {
        get => this.text;
        set => this.SetProperty(ref this.text, value);
    }

    private double progress;
    public double Progress
    {
        get => this.progress;
        set => this.SetProperty(ref this.progress, Math.Clamp(value, 0.0, 1.0));
    }

    public void Cancel()
    {
        this.cancelAction?.Invoke();
    }
}
