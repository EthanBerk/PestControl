using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Turrets
{
    public enum turretState
    {
        blue,
        red,
        yellow,
        orange,
        purple,
        green
    }

    public class Turret : MonoBehaviour
    {
        public GameObject[] levels;
        public turretState State;
        private BasicGun _basicGun;
        private GameObject gun;
        public BasicGun gunInfo { get; set; }
        public Vector2 size { get; set; }
        public int level { get; set; } = 0;

        private void Start()
        {
            updateTurret();
        }

        private void Update()
        {
        }

        public void updateTurret()
        {
            Destroy(gun);
            gun = Instantiate(levels[level], transform.position, Quaternion.identity, transform);
            gunInfo = gun.GetComponentInChildren<BasicGun>();
        }
    }
}