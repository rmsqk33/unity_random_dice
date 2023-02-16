using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace RandomDice
{
    public class Bullet : MonoBehaviour
    {
        // - Member Variable
        // - Image, Target
        public Image _uiImage;
        public GameObject _Target;

        // - BulletInfo
        public struct BULLET_INFO
        {
            public bool _Islive;
            public int _Damage;
            public float _Speed;
            public Vector3 _Direction;
        }

        private BULLET_INFO _info = new BULLET_INFO();

        // - Method
        // - Base
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
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
            gameObject.transform.position += _info._Direction * _info._Speed;
        }
        // - Attack
    }
}