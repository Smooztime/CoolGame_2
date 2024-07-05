using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunSystem : MonoBehaviour
{
    [Header("-----Muzzle Flash-----")]
    [SerializeField]
    private Transform _muzzleFlashPosition;
    [SerializeField]
    private string muzzleFlashName;
    [SerializeField]
    private TrailRenderer tracerEffect;

    [Header("-----Gun Stat-----")]
    [SerializeField]
    private float gunDamage;
    [SerializeField]
    private float spread;
    [SerializeField]
    private float range;
    [SerializeField]
    private int amountOfProjectile;
    [SerializeField]
    private LayerMask _target;
    [SerializeField]
    private AudioClip gunAudioClip;
    [SerializeField]
    private float gunVolume = 1;
    [SerializeField]
    private bool holdShoot;

    public virtual void Shoot()
    {
        MuzzleFlash _muzzleFlash = (MuzzleFlash)PoolManager.Instance.Spawn(muzzleFlashName);
        _muzzleFlash.transform.position = _muzzleFlashPosition.position;
        _muzzleFlash.transform.rotation = _muzzleFlashPosition.rotation;
        _muzzleFlash.transform.SetParent(_muzzleFlashPosition.transform);
        SoundManager.PlaySound(gunAudioClip, gunVolume);

        for (int i = 0; i < amountOfProjectile; i++)
        {
            ShootingRay();
        }
    }

    private void ShootingRay()
    {
        Vector3 direction = _muzzleFlashPosition.right;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        direction += new Vector3(x, y, 0); 

        RaycastHit2D hit = Physics2D.Raycast(_muzzleFlashPosition.position, direction, range, _target);
        TrailRenderer tracer = Instantiate(tracerEffect, _muzzleFlashPosition.position, Quaternion.identity);
        tracer.AddPosition(_muzzleFlashPosition.position);

        if (!holdShoot)
        {
            SoundManager.PlayLoop(gunAudioClip);
            SoundManager.AdjustSFXVolume(1f);

            if (hit.collider != null)
            {
                tracer.transform.position = hit.point;
                if (hit.collider.TryGetComponent(out EnemySpawner spawnerDamage))
                {
                    spawnerDamage.DamageToSpawner(gunDamage);
                }
                if (hit.collider.TryGetComponent(out Enemy enemyDamage))
                {
                    enemyDamage.DamageToEnemy(gunDamage);
                }
            }
            else
            {
                Vector3 pos = _muzzleFlashPosition.position + direction * range;
                tracer.transform.position = pos;
            }
        }
        else
        {
            if (hit.collider != null)
            {
                tracer.transform.position = hit.point;
                if (hit.collider.TryGetComponent(out EnemySpawner spawnerDamage))
                {
                    spawnerDamage.DamageToSpawner(gunDamage * Time.fixedDeltaTime);
                }
                if (hit.collider.TryGetComponent(out Enemy enemyDamage))
                {
                    enemyDamage.DamageToEnemy(gunDamage * Time.fixedDeltaTime);
                }
            }
            else
            {
                Vector3 pos = _muzzleFlashPosition.position + direction * range;
                tracer.transform.position = pos;
            }
        }
    }

    public bool GetHoldShoot()
    {
        return holdShoot;
    }
}
