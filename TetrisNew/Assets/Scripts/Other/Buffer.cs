using UnityEngine;

public static class Buffer
{
    internal static void BuffSome(PrefsSave playerPrefs, float value = 1)
    {
        string path = playerPrefs.ToString();
        value += PlayerPrefs.GetFloat(path);
        PlayerPrefs.SetFloat(path, value);
    }
}
