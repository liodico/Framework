using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace WorldEditor
{
    public class EasyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public TextMeshProUGUI txtContent;

        Image img;
        Image Img
        {
            get
            {
                if (img is null) img = GetComponent<Image>();
                return img;
            }
        }

        Material greyMat;
        Material GreyMat
        {
            get
            {
                if (greyMat is null) greyMat = Resources.Load<Material>("Materials/GreyEffect");
                var obj = Instantiate(greyMat);
                return obj;
            }
        }

        void Awake()
        {
            // Img.material = GreyMat;
            // Img.material.SetColor("blendColor", Img.color);
        }

        bool enable = true;
        public bool Enable
        {
            get => enable;
            set
            {
                // Img.material.SetFloat("flyColor", value ? 0 : 1);
                enable = value;
            }
        }

        float scaleRatio = 0.9f;
        int scaleFlow = 0;

        Action callBack;

        public void OnClick(Action act)
        {
            callBack = act;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !enable) return;
            callBack?.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !enable) return;
            scaleFlow = -1;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !enable) return;
            scaleFlow = 1;
        }

        void Update()
        {
            if (scaleFlow < 0)
            {
                transform.localScale -= new Vector3(0.02f, 0.02f, 0);
                if (transform.localScale.x < scaleRatio)
                {
                    transform.localScale = new Vector3(1, 1, 1) * scaleRatio;
                    scaleFlow = 0;
                }
            }
            else if (scaleFlow > 0)
            {
                transform.localScale += new Vector3(0.02f, 0.02f, 0);
                if (transform.localScale.x > 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    scaleFlow = 0;
                }
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            // if (Img.material == null) Img.material = GreyMat;
            // Img.material.SetColor("blendColor", Img.color);
        }
#endif
    }
}