using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : PoolObject
{
    private void OnEnable()
    {
        Invoke(nameof(DeSpawn), 0.5f);
    }
}
