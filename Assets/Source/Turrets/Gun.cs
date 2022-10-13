using System;
using UnityEngine;

namespace Source.Turrets
{
    public class Gun : MonoBehaviour
    {
        public Vector2 rangeNorm;
        public Vector2 rangeBuff;
        public Vector2 range { get; set; }


        public float RotationSpeedNorm;
        public float RotationSpeedBuff;
        public float RotationSpeed { get; set; }

        public float DamageNorm;
        public float DamageBuff;
        public float Damage { get; set; }

        public GameObject firePoint;

        public Collider2D attacking { get; set; }
        public LayerMask enemyMask { get; set; }
        public bool canHitFlying;

        public virtual void Start()
        {
            var layer1 = 1 << 7;
            var layer2 = 1 << 7;
            if (canHitFlying)
            {
                layer2 = 1 << 8;
            }

            enemyMask = layer1 | layer2;
            Damage = DamageNorm;
            range = rangeNorm;
            RotationSpeed = RotationSpeedNorm;
        }


        public virtual void UpdateAttacking()
        {
            var hits = Physics2D.OverlapBoxAll(transform.position, range, 0, enemyMask);
            foreach (var hit in hits)
            {
                if (!attacking)
                {
                    attacking = hit;
                }

                if (hit == attacking)
                {
                    attacking = hit;
                    return;
                }
            }

            attacking = null;
        }

        public void RotateToAttacking()
        {
            var angle = Mathf.Atan2(attacking.transform.position.y - transform.position.y,
                attacking.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        public virtual void Buff()
        {
            range = rangeBuff + rangeNorm;
            RotationSpeed = RotationSpeedNorm + RotationSpeedBuff;
            Damage = DamageNorm + DamageBuff;
        }

        public virtual void DeBuff()
        {
            range = rangeNorm;
            RotationSpeed = RotationSpeedNorm;
            Damage = DamageNorm;
        }

        public virtual void onHit(Collider2D hit)
        {
            var enemy = hit.GetComponent<Enemy>();
            enemy.health -= Damage;
        }
    }
}