using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace WorldEditor
{
    public class ObjectPrefab : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField] ObjectData objectData;
        public int type;
        public int id;
        [SerializeField] EasyButton btnX;
        public TMP_InputField ebTimeAppear;
        public TMP_InputField ebTimeIdle;
        [SerializeField] Image image;

        void Start()
        {
            btnX.OnClick(OnClickX);
        }

        void OnClickX()
        {
            gameObject.SetActive(false);
        }

        public void Set(int type, int id, float timeAppear = 0, float timeIdle = 0)
        {
            this.type = type;
            this.id = id;
            ebTimeAppear.text = "" + timeAppear;
            ebTimeIdle.text = "" + timeIdle;
            image.sprite = type == ObjectData.TYPE_OBJECT ? objectData.arrObject[id] : objectData.arrUnit[id];
        }

        Vector3 vector;

        public void OnBeginDrag(PointerEventData eData)
        {
            vector = transform.position - (new Vector3(eData.position.x, eData.position.y, 0) - new Vector3(960, 540, 0));
        }

        public void OnDrag(PointerEventData eData)
        {
            transform.position = new Vector3(eData.position.x, eData.position.y, 0) - new Vector3(960, 540, 0) + vector;
        }
    }
}