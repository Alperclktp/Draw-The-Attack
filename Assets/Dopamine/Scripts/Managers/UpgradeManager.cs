using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public int upgradeCount;
    public void IncreaseUpgradeCount()
    {
        upgradeCount += 1;
    }
}
