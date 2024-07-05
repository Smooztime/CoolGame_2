using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField]
    private float weaponTime;
    [SerializeField]
    private float shootTime;
    [SerializeField]
    private float weaponSpeed;
    [SerializeField]
    private Transform weaponPoint;
    
    private Transform aimTransform;
    private Vector3 _aimDir;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        aimTransform = transform.Find("SkeletonAim");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        AimWeapon();
        EnemyChase();

        weaponTime += Time.fixedDeltaTime;
        if(weaponTime >= shootTime)
        {
            ThrowWeapon();
            weaponTime = 0;
        }
    }

    protected void ThrowWeapon()
    {
        Bone bone = (Bone)PoolManager.Instance.Spawn(enemyWeapon);
        bone.transform.position = weaponPoint.position;
        bone.transform.rotation = weaponPoint.rotation;

        Rigidbody2D rb = bone.GetComponent<Rigidbody2D>();
        rb.AddForce(_aimDir * EnemySpeed * 5, ForceMode2D.Impulse);
    }

    private void AimWeapon()
    {
        _aimDir = player.transform.position - transform.position;
        _aimDir.Normalize();
        float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }
}
