using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfigurationData", menuName = "LevelConfigurationData")]
public class LevelConfigurationData : ScriptableObject
{
    [Header("Spawnable Objects")]
    public List<GameObject> gameobjectsPreview;

    public List<string> emptyCreated;
}
