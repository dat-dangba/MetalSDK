using System;
using System.Collections.Generic;

[Serializable]
public class WinCountConfig
{
    public int FromLevel;
    public int ToLevel;
    public int WinCount;

    public bool Contains(int level) => level >= FromLevel && (ToLevel <= 0 || level <= ToLevel);
}

[Serializable]
public class WinCountAdConfig : AdConfig
{
    public List<WinCountConfig> WinCountConfigs = new();

    public bool CanShow(int level)
    {
        foreach (var item in WinCountConfigs)
        {
            if (item.Contains(level)) return (level - item.FromLevel) % item.WinCount == 0;
        }

        return false;
    }
}
