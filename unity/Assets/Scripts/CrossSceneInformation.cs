using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossSceneInformation
{
    public static int MaxLevel = 5;
    public static int Level = 1;

    public static int PlayerHealth = 100;
    public static int PlayerMaxHealth = 100;

    public static bool PlayerHeavyUnlocked = false;
    public static bool PlayerAssaultUnlocked = false;
    public static bool PlayerShotgunUnlocked = false;
    public static bool PlayerLaserUnlocked = false;

    public static int[] AmmoCount;

    static CrossSceneInformation() {
        CrossSceneInformation.ResetValues();
    }

    public static void ResetValues()
    {
        MaxLevel = 1; // TODO: Change to higher value
        Level = 1;

        PlayerHealth = 100;
        PlayerMaxHealth = 100;

        PlayerHeavyUnlocked = false;
        PlayerAssaultUnlocked = false;
        PlayerShotgunUnlocked = false;
        PlayerLaserUnlocked = false;

        AmmoCount = null;
    }
}
