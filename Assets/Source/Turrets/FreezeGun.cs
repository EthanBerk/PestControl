using System.Collections;
using UnityEngine;

namespace Source.Turrets
{
    public class FreezeGun : RedGun
    {
        public float freezeTimeNorm = 0.2f;
        public float freezeTimeBuff = 0;
        public float freezeTime { get; set; }

        float speedSetNorm;
        public float speedSetBuff;
        public float SpeedSet { get; set; }

        public bool doesDamage;

        public override void Start()
        {
            freezeTime = freezeTimeNorm;
            SpeedSet = speedSetNorm;
            base.Start();
        }

        public override void Buff()
        {
            SpeedSet = speedSetNorm + speedSetBuff;
            freezeTime = freezeTimeNorm + freezeTimeBuff;
            base.DeBuff();
        }

        public override void DeBuff()
        {
            freezeTime = freezeTimeNorm;
            SpeedSet = speedSetNorm;
            base.DeBuff();
        }

        public override void onHit(Collider2D hit)
        {
            var enemy = hit.GetComponent<Enemy>();
            enemy.frozenSpeed = SpeedSet;
            StartCoroutine(freeze(enemy));
            if (doesDamage)
            {
                base.onHit(hit);
            }
        }

        private IEnumerator freeze(Enemy enemy)
        {
            enemy.frozen++;
            yield return new WaitForSeconds(freezeTime);
            enemy.frozen--;
       }
    }
}