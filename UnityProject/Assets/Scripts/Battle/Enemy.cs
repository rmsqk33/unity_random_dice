using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace RandomDice
{
    public class Enemy : MonoBehaviour
    {
        // - Member Variable
        // - Image, HPText
        public Image _uiImage;
        public TextMeshProUGUI _uiHP;

        // - EnemyInfo : islive, hp, movespeed, movedirection
        public struct ENEMY_INFO
        {
            public bool _islive;
            public int _hp;
            public float _speed;
            public Vector3 _direction;
        }

        private ENEMY_INFO _info = new ENEMY_INFO();


        // - Mothod
        // - base
        private void FixedUpdate()
        {
            // - Move
            Move();
        }

        // - Get & Set
        public void SetHp(int hp)
        {
            _info._hp = hp;
        }
        public void SetSpeed(float speed)
        {
            _info._speed = speed;
        }

        // - TakeDamage
        public void TakeDamage(int damage)
        {
            _info._hp -= damage;
            if (_info._hp <= 0)
            {
                _info._islive = false;
                Destroy(gameObject);
            }
        }
        // - Move
        private void Move()
        {
            if (_info._islive)
            {
                gameObject.transform.position += _info._direction * _info._speed;
            }
        }
    }
}
