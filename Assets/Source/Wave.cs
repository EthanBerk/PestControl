using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    [Serializable]
    public class Wave
    {
        public int stage;
        public List<EnemySpawn> Spawns;
    }
}