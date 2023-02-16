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


// - Method
    // - Base
    // Start is called before the first frame update
    void Start()
    {
        
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
}
