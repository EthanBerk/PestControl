using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source
{
    public class Enemy : MonoBehaviour
    {
        public GameObject pathDir { get; set; }
        private float speedDown;
        private float speedLeft;
        public float frozenSpeed { get; set; }
        public float normSpeedDown;
        public float normSpeedleft;
        public float Tolerance = 0.1f;
        private List<Transform> path;
        public int index { get; set; }
        public float health;
        public int damage;

        public int worth;

        public GameManger _gameManger { get; set; }

        private float _dyeTime;
        private float timeToDye = 1;
        private bool dying;
        private SpriteRenderer _spriteRenderer;

        public int healthAddPerStage;

        private Animator _animator;
        private Animator _helthBaranimator;
        public GameObject healthBar;

        public int frozen { get; set; }
        private bool down;


        public List<EnemySpawn> spawnOnDeath;

        private bool HaveDamaged;
        private float maxHealth;


        private void Start()
        {
            maxHealth = health;

            _animator = gameObject.GetComponent<Animator>();
            path = pathDir.GetComponentsInChildren<Transform>().ToList();
            path.RemoveAt(0);
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            _helthBaranimator = healthBar.GetComponent<Animator>();
        }

        public void UpdateHelh()
        {
            if (_gameManger.stage >= 20)
            {
                health += _gameManger.stage * healthAddPerStage;
                maxHealth = health;
            }
        }

        private void Update()
        {
            _helthBaranimator.SetFloat("Blend", health / maxHealth);
            _animator.SetBool("Down", down);
            if (frozen > 0)
            {
                speedLeft = frozenSpeed;
                speedDown = frozenSpeed;
                _spriteRenderer.color = Color.blue;
            }
            else
            {
                _spriteRenderer.color = Color.white;
                speedLeft = normSpeedleft;
                speedDown = normSpeedDown;
            }

            if (dying)
            {
                var value = ((Time.time - _dyeTime) / timeToDye - 1) * -1;
                if (value <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _spriteRenderer.color = new Color(1, 1, 1, value);
                }
            }
            else
            {
                if (health <= 0)
                {
                    death();
                }
                else if (index == 7 && !HaveDamaged)
                {
                    _gameManger.damageBase(this);
                    HaveDamaged = true;
                }

                transform.Translate((path[index + 1].position - transform.position).normalized *
                                    ((down ? speedDown : speedLeft) * Time.deltaTime));
                if (Math.Abs(transform.position.x - path[index + 1].position.x) < Tolerance &&
                    Math.Abs(transform.position.y - path[index + 1].position.y) < Tolerance)
                {
                    index++;
                }

                transform.localScale =
                    new Vector3(((path[index + 1].position - transform.position).x < 0) ? -1 : 1, 1, 1);
                healthBar.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
                down = Mathf.Abs((path[index + 1].position - transform.position).x) <
                       Mathf.Abs((path[index + 1].position - transform.position).y);
            }
        }

        private void death()
        {
            _gameManger.removeEnemy(this);
            _gameManger.money += worth;
            _dyeTime = Time.time;
            dying = true;
            if (spawnOnDeath.Any())
            {
                StartCoroutine(_gameManger.SpawnGroup(spawnOnDeath, transform.position, index));
            }
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}