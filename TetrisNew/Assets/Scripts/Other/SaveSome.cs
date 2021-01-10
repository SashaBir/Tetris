using UnityEngine;

internal static class SaveSome
{
    internal static void DeletAllKeys() => PlayerPrefs.DeleteAll();

    internal static float GetParameter(PrefsSave item, float value = 0)
    {
        string path = item.ToString();
        
        if (PlayerPrefs.HasKey(path))
            value = PlayerPrefs.GetFloat(path);
        else
            PlayerPrefs.SetFloat(path, value);

        return value;
    }
    
    internal static string GetParameter(PrefsSave item, string value = "") //string value = null
    {
        string path = item.ToString();

        if (PlayerPrefs.HasKey(path))
            value = PlayerPrefs.GetString(path);
        else
            PlayerPrefs.SetString(path, value);

        return value;
    }
    
    internal static string GetParameter(PrefsSave item)
    {
        return PlayerPrefs.GetString(item.ToString());
    }

    internal static void SetParametr(PrefsSave item, float value) => PlayerPrefs.SetFloat(item.ToString(), value);
}