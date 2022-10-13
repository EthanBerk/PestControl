using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


namespace Source
{
    public class GameManger : MonoBehaviour
    {
        public GameObject pathDir;
        private List<Transform> path;
        public List<Wave> CustomWaves;
        public List<EnemySpawnInfo> EnemySpawnsInfos;
        public GameObject prefab;

        public Text HelthText;


        public GameObject endScreen;
        public Text EndScreenText;

        public float defaultSpawnRate;
        public Button next;
        public bool canMoveToNextStage { get; set; }

        public float fastFowardScale = 2;

        public int baseHealth;

        public int money;
        public Text moneyText;
        public Text LevelText;

        public int stage;
        public List<GameObject> enemies { get; set; } = new List<GameObject>();


        private void Start()
        {
            Time.timeScale = 1;
            path = pathDir.GetComponentsInChildren<Transform>().ToList();
            path.RemoveAt(0);
        }

        private bool HghScore;

        private bool screen;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!HghScore)
                {
                    if (!screen)
                    {
                        Time.timeScale = 0;
                        endScreen.SetActive(true);
                        screen = true;
                    }
                    else
                    {
                        Time.timeScale = 1;
                        endScreen.SetActive(false);
                        screen = false;
                    }
                }
            }

            LevelText.text = "STAGE: " + (stage + 1);
            HelthText.text = baseHealth.ToString();
            if (baseHealth <= 0)
            {
                canMoveToNextStage = false;
                Time.timeScale = 0;
                if (!HghScore)
                {
                    if (PlayerPrefs.GetInt("High") < stage)
                    {
                        endScreen.SetActive(true);
                        EndScreenText.text = "NEW HIGH SCORE: " + stage;
                        HghScore = true;

                        PlayerPrefs.SetInt("High", stage);
                    }
                    else
                    {
                        endScreen.SetActive(true);
                        EndScreenText.text = "SCORE: " + stage;
                        HghScore = true;
                    }
                }
            }

            moneyText.text = money + " $";
            next.interactable = canMoveToNextStage;

            canMoveToNextStage = enemies.Count == 0;
        }

        private void SpawnWave(int stage)
        {
            foreach (var wave in CustomWaves.Where(wave => wave.stage == stage))
            {
                StartCoroutine(SpawnGroup(wave.Spawns, path[0].position, 0));
                return;
            }

            var listOfSpawns = (from enemy in EnemySpawnsInfos
                    where stage > enemy.stageStart & stage < enemy.stageEnd
                    select new EnemySpawn(defaultSpawnRate, enemy.State, enemy.spawnScale * (stage - enemy.stageStart)))
                .ToList();
            StartCoroutine(SpawnGroup(listOfSpawns, path[0].position, 0));
        }

        public IEnumerator SpawnGroup(List<EnemySpawn> spawns, Vector2 pos, int index)
        {
            foreach (var spawn in spawns)
            {
                for (int i = 0; i < spawn.amount; i++)
                {
                    var spawnInfo = getSpawnInfo(spawn.State);
                    var cu = Instantiate(spawnInfo.prefab, pos, quaternion.identity,
                        gameObject.transform);
                    enemies.Add(cu);
                    cu.GetComponent<Enemy>().pathDir = pathDir;
                    cu.GetComponent<Enemy>()._gameManger = this;
                    cu.GetComponent<Enemy>().index = index;
                    cu.GetComponent<Enemy>().UpdateHelh();
                    yield return new WaitForSeconds(spawn.rate);
                }
            }
        }

        private EnemySpawnInfo getSpawnInfo(enemyState state)
        {
            foreach (var spawnInfo in EnemySpawnsInfos.Where(spawnInfo => spawnInfo.State == state))
            {
                return spawnInfo;
            }

            throw new IndexOutOfRangeException("f you henry");
        }

        public void damageBase(Enemy enemy)
        {
            baseHealth -= enemy.damage;
            enemies.Remove(enemy.gameObject);
        }

        public void removeEnemy(Enemy enemy)
        {
            enemies.Remove(enemy.gameObject);
        }

        private bool Once;

        public void moveToNextStage()
        {
            if (stage != 0 || Once)
            {
                stage++;
            }
            else
            {
                Once = true;
            }

            SpawnWave(stage);
        }

        public void fastFoward()
        {
            if (Time.timeScale == fastFowardScale)
            {
                Time.timeScale = 1;
            }
            else if (Time.timeScale == 1)
            {
                Time.timeScale = fastFowardScale;
            }
        }
    }
}