using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;
using static RandomDice.Bullet;

public class BulletManager : Singleton<BulletManager>
{
// - Member Variable
    // - BulletList, 
    private List<Bullet> _bullets = new List<Bullet>();
    private const int BULLET_MAX = 1000;
    private int _index = 0;

// - Method
    // - Base
    // Start is called before the first frame update
    void Start()
    {
        // - Bullet Setup 1000
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // - Original
    public void AddBullet(BULLET_INFO info)
    {
        Bullet bullet = new Bullet();
    }

    public void Fire( Vector3 start, GameObject target )
    {
        // - Get Bullet

        // - Set Bullet

        // - Fire
    }

    // - GetBullet

}
