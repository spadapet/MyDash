using System;
using System.Text.Json;

namespace MyDash.Data.Model;

public sealed class Settings : PropertyNotifier, ICloneable
{
    object ICloneable.Clone()
    {
        return this.Clone();
    }

    public Settings Clone()
    {
        Settings clone = new Settings();
        clone.CopyFrom(this);
        return clone;
    }

    public void CopyFrom(Settings source)
    {
        this.Organization = source.Organization;
    }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
    }

    public static Settings Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<Settings>(json);
        }
        catch
        {
            return new Settings();
        }
    }

    private string organization;
    public string Organization
    {
        get => this.organization;
        set => this.SetProperty(ref this.organization, value);
    }
}
