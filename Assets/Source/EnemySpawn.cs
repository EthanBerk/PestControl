using System;
using UnityEngine;

namespace Source
{
    public enum enemyState
    {
        snail,
        slug,
        rolyPoly,
        ladyBug,
        ant,
        bee,
        beetle,
        dragonFly,
        quuenAnt
    }

    [Serializable]
    public class EnemySpawn
    {
        public float rate = 0.5f;
        public enemyState State;
        public int amount;

        public EnemySpawn(float rate, enemyState state, int amount)
        {
            this.rate = rate;
            State = state;
            this.amount = amount;
        }
    }
}