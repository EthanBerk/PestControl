using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace Source.Turrets
{
    public class BasicGun : Gun
    {
        private bool canShoot = true;

        public GameObject bulletPrefab;

        public float fireRateNorm;
        public float fireRateBuff;
        public float fireRate { get; set; }

        public float HitsNorm;
        public float HitsBuff;
        public float Hits { get; set; }

        public override void Start()
        {
            Hits = HitsNorm;
            fireRate = fireRateNorm;
            base.Start();
        }

        public void Update()
        {
            UpdateAttacking();
            if (attacking)
            {
                RotateToAttacking();
                if (canShoot)
                {
                    shoot();
                    StartCoroutine(updateShoot());
                }
            }
        }

        public void shoot()
        {
            var bullet = Instantiate(bulletPrefab, firePoint.transform.position, transform.rotation)
                .GetComponent<Bullet>();
            bullet._basicGun = this;
        }

        public IEnumerator updateShoot()
        {
            canShoot = false;
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        public override void Buff()
        {
            fireRate = fireRateNorm - fireRateBuff;
            Hits = HitsNorm - HitsBuff;
            base.Buff();
        }

        public override void DeBuff()
        {
            fireRate = fireRateNorm;
            Hits = HitsNorm;
            base.DeBuff();
        }
    }
}