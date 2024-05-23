using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Player/Skills")]
public class SkillScriptable : ScriptableObject
{
    public string skillName;
    public float skillCost;
    public float skillCooldown;
    public SpriteRenderer skillIcon;
}
