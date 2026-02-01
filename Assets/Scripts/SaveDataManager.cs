using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public static class SaveDataManager
{
    private static readonly string SavePath =
        Path.Combine(Application.persistentDataPath, "pro.json");

    [Serializable]
    private class SaveEntry
    {
        public string key;
        public int value;
    }

    [Serializable]
    private class SaveData
    {
        public List<SaveEntry> entries = new List<SaveEntry>();
    }

    private static Dictionary<string, int> data = new Dictionary<string, int>();
    private static bool isLoaded = false;

    // =========================
    // Public API
    // =========================

    public static void Save(string key, int value)
    {
        if (string.IsNullOrEmpty(key)) return;

        LoadIfNeeded();
        data[key] = value;
        Debug.Log($"[SaveDataManager.Save] data[{key}] = {value}");
        WriteFile();
    }

    public static bool Load(string key, out int value)
    {
        LoadIfNeeded();
        return data.TryGetValue(key, out value);
    }

    public static int Load(string key, int defaultValue)
    {
        LoadIfNeeded();
        return data.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public static bool HasKey(string key)
    {
        LoadIfNeeded();
        return data.ContainsKey(key);
    }

    public static void DeleteAll()
    {
        data.Clear();
        WriteFile();
    }

    // =========================
    // Internal
    // =========================

    private static void LoadIfNeeded()
    {
        if (isLoaded) return;

        data.Clear();

        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData != null)
            {
                foreach (var entry in saveData.entries)
                {
                    data[entry.key] = entry.value;
                }
            }
        }

        isLoaded = true;
    }

    private static void WriteFile()
    {
        SaveData saveData = new SaveData();

        foreach (var pair in data)
        {
            saveData.entries.Add(new SaveEntry
            {
                key = pair.Key,
                value = pair.Value
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"SaveData written: {SavePath}\n{json}");
    }
}
