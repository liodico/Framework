using UnityEngine;
using UnityEngine.UI;

namespace WorldEditor
{
    public class ObjectSlot : MonoBehaviour
    {
        [SerializeField] ObjectData objectData;
        public int type;
        public int id;
        public EasyButton button;

        [SerializeField] Image image;

        public void Set(int type, int id)
        {
            this.type = type;
            this.id = id;
            image.sprite = type == ObjectData.TYPE_OBJECT ? objectData.arrObject[id] : objectData.arrUnit[id];
        }
    }
}