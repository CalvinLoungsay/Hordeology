using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxDist = 1.0f;
    public float targetUpdateTimer = 0.1f;
    [Header("Shooting")]
    public float attackSpread = 0;
}
