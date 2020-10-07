using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldEditor
{
    [CreateAssetMenu(fileName = "ObjectData", menuName = "ScriptableObjects/ObjectData")]
    public class ObjectData : ScriptableObject
    {
        public const int TYPE_OBJECT = 0;
        public const int TYPE_UNIT = 1;
        public Sprite[] arrObject;
        public Sprite[] arrUnit;
    }
}