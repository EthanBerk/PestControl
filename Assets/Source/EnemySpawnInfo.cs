using System;
using UnityEngine;

namespace Source
{
    [Serializable]
    public class EnemySpawnInfo
    {
        public enemyState State;
        public int stageStart;
        public int stageEnd;
        public int spawnScale;
        public GameObject prefab;
    }
}