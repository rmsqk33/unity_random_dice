using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomDice
{
    public enum TYPE
    {
        NORMAL = 0,
        FIRE,
        END
    }

    public class TypeManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    // - Method
        // - GetTypeName
        public string GetTypeName(TYPE type)
        {
            return type.ToString(); 
        }

        // - GetTypeObject
        // - Load DucePrefab by Type
        // - Load BulletPrefab by Type
        // -
    }

}