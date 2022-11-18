using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
// - Member Variable
    public Image    _uiSprite;
    public Text     _uiText;  

    private bool    _islive = true;
    private int     _hp = 100;
    private float   _speed = 1.0f;

// - Mothod
    public void SetHp(int hp)
    {
        _hp = hp;
    }
    public void SetSpeed(float speed) 
    {
        _speed = speed;
    }
    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if( _hp <= 0)
        {
            _islive = false;
        }
    }

    void Update() 
    {

    }
}
