using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using RandomDice;

public class Dice : MonoBehaviour
{
// - Member Variable
    // - DiceImage, Attacker
    public Image            _uiImage;
    public int              _grade = 1;
    public GameObject[]     _Eyes = new GameObject[MAX_EYES];
    public GameObject       _TargetEnemy;
    public int              _CurrentEyeIndex = 0;
    const int               MAX_EYES = 7;
    public                  TYPE _type = TYPE.NORMAL;


    // - DiceInfo : State, Damage, Skill, 
    public struct DICE_INFO
    {
        public int      _Damage;
        public float    _AttackSpped;
        public bool     _isPlayer;
    }


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

    // - SetEyes
    public void SetEyes()
    {

    }

    // - Bullet Fire
    public void Fire(GameObject target)
    {
        BulletManager.Instance.Fire(_Eyes[_CurrentEyeIndex].transform.localPosition, target);

    }

}
