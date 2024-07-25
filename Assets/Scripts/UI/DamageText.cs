using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : PoolObject
{
    private void OnEnable()
    {
        Invoke(nameof(DeSpawn), 1f);
    }
}
