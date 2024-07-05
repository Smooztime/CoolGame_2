using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkMuzzleFlash : PoolObject
{
    private void OnEnable()
    {
        Invoke(nameof(DeSpawn), 0.1f);
    }
}
