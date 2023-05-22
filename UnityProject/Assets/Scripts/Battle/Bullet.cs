using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace RandomDice
{
    // - BulletInfo
    public struct BULLET_INFO
    {
        public bool     _Islive;
        public int      _Damage;
        public float    _Speed;
        public Vector3  _Direction;
    }

    public class Bullet : MonoBehaviour
    {
        // - Member Variable
        public Image        _uiImage;
        public GameObject _Target;

        private BULLET_INFO _info = new BULLET_INFO();

        // - Constructor
        public Bullet()
        {

        }
        public Bullet(GameObject target, BULLET_INFO info)
        {
            _info = info;
        }

    // - Method
        // - Base
        private void Awake()
        {
            // - Initiation
            
        }
        void Start()
        {

        }

        private void FixedUpdate()
        {
            // - BulletMove
            Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }

        // - Move
        private void Move()
        {
            // - gameObject active check
            if (gameObject.active == true)
            {
                gameObject.transform.position += _info._Direction * _info._Speed;
            }
        }
        // - Attack
    }
}