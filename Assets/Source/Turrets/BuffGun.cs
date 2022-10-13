using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Turrets
{
    public class BuffGun : Gun
    {
        public override void Start()
        {
            base.Start();
            var ring = Instantiate(buffRing, transform);
            ring.transform.localPosition = centerOfBuff;

            buffRingRenderer = ring.GetComponent<SpriteRenderer>();
            buffRingRenderer.size = range;
        }

        private void Update()
        {
            UpdateBuffing();
        }

        public LayerMask turretMask;
        private List<Gun> buffed = new List<Gun>();
        private List<Collider2D> buffedColiders = new List<Collider2D>();
        public GameObject buffRing;
        private SpriteRenderer buffRingRenderer;

        public Vector2 centerOfBuff;

        private void UpdateBuffing()
        {
            var hits = Physics2D.OverlapBoxAll(transform.position + (Vector3) centerOfBuff,
                range - new Vector2(0.1f, 0.1f), 0, turretMask);
            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Turret")) continue;
                if (buffedColiders.Contains(hit)) continue;
                var gun = hit.GetComponentInChildren<Gun>();
                gun.Buff();
                buffedColiders.Add(hit);
                buffed.Add(gun);
            }

            for (var i = 0; i < buffedColiders.Count; i++)
            {
                var buffedColider = buffedColiders[i];
                if (hits.Contains(buffedColider)) continue;
                var gun = buffedColider.GetComponentInChildren<Gun>();
                gun.DeBuff();
                buffedColiders.Remove(buffedColider);
                i--;
                buffed.Remove(gun);
            }
        }
    }
}