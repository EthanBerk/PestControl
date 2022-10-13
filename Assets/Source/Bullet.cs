using System.Collections.Generic;
using Source.Turrets;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BasicGun _basicGun { get; set; }
    private int hits;
    private List<Collider2D> hasHit = new List<Collider2D>();

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, _basicGun.enemyMask);
        if (hit)
        {
            if (!hasHit.Contains(hit))
            {
                _basicGun.onHit(hit);
                hasHit.Add(hit);
                if (hits >= _basicGun.Hits)
                {
                    Destroy(gameObject);
                }
                else
                {
                    hits++;
                }
            }
        }

        transform.Translate(0, 10 * Time.deltaTime, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}