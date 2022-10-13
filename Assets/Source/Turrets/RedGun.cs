using System;
using UnityEngine;

namespace Source.Turrets
{
    public class RedGun : Gun
    {
        public ParticleSystem fire;

        private void Update()
        {
            UpdateAttacking();
            if (attacking)
            {
                RotateToAttacking();
                fire.Play();
            }
            else
            {
                fire.Stop();
            }
        }
    }
}