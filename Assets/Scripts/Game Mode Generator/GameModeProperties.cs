using UnityEngine;

public class GameModeProperties : ScriptableObject
{
    public GameMode gm;
    public Perspective pers;
    public ObjectiveEndless objEndless;
    public ObjectivePlatformer objPlatform;
    public ObjectiveSurvival objSurvival;
    public int amountOfLevels;
    public float limitTime;

}
public enum Perspective
{
    side,
    iso,
    third,
    top
}
public enum GameMode
{
    platform,
    endless,
    survival
}

public enum ObjectivePlatformer
{
    GetToPointB,
    CollectCoins,
    BYTIME,
    BYPOINTS,
     BYKILLING
}
public enum ObjectiveEndless
{
    BYTIME,
    BYPOINTS
}
public enum ObjectiveSurvival
{
    BYTIME,
    BYKILLING
}


