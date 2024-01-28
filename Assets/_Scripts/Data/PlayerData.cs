using Dasis.Extensions;
using System;
using UnityEngine;

public static class PlayerData
{
    private static readonly string prefix = "_";

    private enum DataName
    {
        HighestScore,
    }

    public static int HighestScore
    {
        get
        {
            return LoadInt($"{DataName.HighestScore}", 0);
        }
        set
        {
            SaveInt($"{DataName.HighestScore}", value);
        }
    }

    #region Save & Load
    private static int LoadInt(string dataName, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt($"{prefix}{dataName}", defaultValue);
    }

    private static void SaveInt(string dataName, int value)
    {
        PlayerPrefs.SetInt($"{prefix}{dataName}", value);
    }

    private static string LoadString(string dataName, string defaultValue = "")
    {
        return PlayerPrefs.GetString($"{prefix}{dataName}", defaultValue);
    }

    private static void SaveString(string dataName, string value)
    {
        PlayerPrefs.SetString($"{prefix}{dataName}", value);
    }
    #endregion
}