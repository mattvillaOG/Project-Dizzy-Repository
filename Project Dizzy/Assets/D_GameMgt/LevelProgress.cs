using UnityEngine;

public static class LevelProgress
{
    private const string Key = "HighestUnlockedBuildIndex";

    // Default: unlock first playable level (build index 2)
    public static int HighestUnlockedBuildIndex
    {
        get => PlayerPrefs.GetInt(Key, 2);
        private set
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
        }
    }

    public static bool IsUnlocked(int buildIndex)
    {
        return buildIndex <= HighestUnlockedBuildIndex;
    }

    public static void UnlockNextFrom(int completedBuildIndex)
    {
        int next = completedBuildIndex + 1;
        if (next > HighestUnlockedBuildIndex)
            HighestUnlockedBuildIndex = next;
    }

    public static void ResetProgress()
    {
        HighestUnlockedBuildIndex = 2;
    }
}