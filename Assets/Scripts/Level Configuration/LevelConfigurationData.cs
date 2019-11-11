using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfigurationData", menuName = "LevelConfigurationData")]
public class LevelConfigurationData : ScriptableObject
{
    [Header("Spawnable Objects")]
    public List<GameObject> gameObjectsPreview;

    public List<string> emptyCreated = new List<string>();
    public List<string> emptyplatformsCreated = new List<string>();
    public List<string> emptyenemiesCreated = new List<string>();
    public List<string> emptyherosCreated = new List<string>();

    public bool previewPowerUp;
    public bool previewPlatform;
    public bool previewEnemies;
}
