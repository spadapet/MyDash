using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MyDash.Data.Model;

public sealed class AppModel : PropertyNotifier, IDisposable
{
    public void Dispose()
    {
        this.adoModel.Dispose();
    }

    private void EnsureValid()
    {
        // I'm valid!
    }

    public Settings Settings { get; } = new Settings();

    private AppState state = AppState.Loading;
    public AppState State
    {
        get => this.state;
        set => this.SetProperty(ref this.state, value);
    }

    private AdoModel adoModel = new();
    public AdoModel AdoModel
    {
        get => this.adoModel;
        set => this.SetProperty(ref this.adoModel, value);
    }

    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public string Serialize()
    {
        this.EnsureValid();
        return JsonSerializer.Serialize(this, AppModel.JsonSerializerOptions);
    }

    public static AppModel Deserialize(string json)
    {
        AppModel model = JsonSerializer.Deserialize<AppModel>(json, AppModel.JsonSerializerOptions);
        model.EnsureValid();
        return model;
    }
}
