using System;
using System.Collections;
using System.Collections.Generic;
using Source.Turrets;
using UnityEngine;

public class Flame : MonoBehaviour
{
    private RedGun _redGun;

    // Start is called before the first frame update
    void Start()
    {
        _redGun = gameObject.GetComponentInParent<RedGun>();
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 7)
        {
            _redGun.onHit(other.GetComponent<BoxCollider2D>());
        }
    }
}