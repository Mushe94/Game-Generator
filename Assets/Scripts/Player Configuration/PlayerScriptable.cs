using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/MyPlayer")]
public class PlayerScriptable : ScriptableObject
{
    public string Name;
    public float Life=1;
    public float speed=1;
    public float damage=1;
    public float jumpForce=1;

    public bool CanJump;
    public bool VerticalMovement;
    public bool HorizontalMovement;
    public bool MeleAttack;

    public int currentDeath;
    //faltan ataques
}