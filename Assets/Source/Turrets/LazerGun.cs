using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Source.Turrets
{
    public class LazerGun : Gun
    {
        public float damageIntervalNorm;
        public float damageIntervalBuff;

        private LineRenderer line;
        public Transform lineEndPoint;

        public GameObject linePrefab;
        public GameObject particlePrefab;

        public float DamageIncNorm;
        public float DamageIncBuff;
        public float DamageInc { get; set; }


        private Vector2 lineDefaultPoint;

        private ParticleSystem LLHitEffectLeft;


        public float damageInterval { get; set; }

        public float maxDistance;

        public override void Start()
        {
            DamageInc = DamageIncNorm;

            LLHitEffectLeft = Instantiate(particlePrefab).GetComponent<ParticleSystem>();
            line = Instantiate(linePrefab).GetComponent<LineRenderer>();

            line.SetPosition(0, firePoint.transform.position);
            base.Start();
        }

        private void OnDestroy()
        {
            if (line == null) return;
            if (LLHitEffectLeft == null) return;
            Destroy(line.gameObject);
            Destroy(LLHitEffectLeft.gameObject);
        }

        public override void DeBuff()
        {
            DamageInc = DamageIncNorm;
            damageInterval = damageIntervalNorm;
            base.DeBuff();
        }

        public override void Buff()
        {
            DamageInc = DamageIncBuff;
            damageInterval = damageIntervalNorm - damageIntervalBuff;
            base.Buff();
        }

        private float laserLength;
        private float startSpriteWidth;
        private float endSpriteWidth;

        private bool damaging;

        public float angle;


        public void Update()
        {
            UpdateAttacking();
            if (attacking)
            {
                RotateToAttacking();


                line.enabled = true;
                line.SetPosition(0, firePoint.transform.position);

                LLHitEffectLeft.Play();
                //LLHitEffectLeft.transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.z - 180, LLHitEffectLeft.transform.eulerAngles.y, LLHitEffectLeft.transform.eulerAngles.z) );


                var hit = Physics2D.Raycast(firePoint.transform.position, (Vector2) (transform.rotation * Vector2.up),
                    maxDistance, enemyMask);
                if (hit)
                {
                    if (!damaging)
                    {
                        damaging = true;
                        var enemy = hit.collider.GetComponent<Enemy>();
                        StartCoroutine(DamageEnemy(enemy, 1));
                    }

                    line.SetPosition(1, hit.point);
                    LLHitEffectLeft.transform.position = hit.point;
                }
                else
                {
                    damaging = false;
                    line.SetPosition(1, lineEndPoint.position);
                    LLHitEffectLeft.transform.position = lineEndPoint.position;
                }
            }
            else
            {
                LLHitEffectLeft.Stop();
                damaging = false;
                line.enabled = false;
            }
        }

        private IEnumerator DamageEnemy(Enemy enemy, int i)
        {
            enemy.health -= Damage + i * DamageInc;
            yield return new WaitForSeconds(damageInterval);
            if (damaging) StartCoroutine(DamageEnemy(enemy, i + 1));
        }
    }
}