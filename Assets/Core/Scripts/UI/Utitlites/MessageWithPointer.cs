using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities.Common;

namespace FoodZombie.UI
{
    public class MessageWithPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform mRectMessage;
        [SerializeField] private TextMeshProUGUI mTxtMessage;
        [SerializeField] private RectTransform mRectPointer;
        [SerializeField] private RectTransform mDialogGroup;

        public int id;
        public RectTransform RectMessage => mRectMessage;
        public RectTransform RectPointer => mRectPointer;

        private void OnDisable()
        {
        }

        public void PointToTarget(RectTransform pTarget, Alignment pAlignment, float pOffset = 0, bool pPostValidate = true)
        {
            mRectPointer.SetActive(true);

            mRectPointer.position = pTarget.position;
            var targetPivot = pTarget.pivot;
            var x = mRectPointer.anchoredPosition.x - pTarget.rect.width * targetPivot.x + pTarget.rect.width / 2f;
            var y = mRectPointer.anchoredPosition.y - pTarget.rect.height * targetPivot.y + pTarget.rect.height / 2f;
            mRectPointer.anchoredPosition = new Vector2(x, y);

            var targetBounds = pTarget.Bounds();
            var arrowBounds = mRectPointer.Bounds();
            var arrowPos = mRectPointer.anchoredPosition;

            switch (pAlignment)
            {
                case Alignment.TopLeft:
                    arrowPos.y = arrowPos.y + targetBounds.size.y / 2 + arrowBounds.size.y / 2 + pOffset;
                    arrowPos.x = arrowPos.x - targetBounds.size.x / 2 - arrowBounds.size.x / 2 - pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, 45));
                    break;
                case Alignment.Top:
                    arrowPos.y = arrowPos.y + targetBounds.size.y / 2 + arrowBounds.size.y / 2 + pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, 0));
                    break;
                case Alignment.TopRight:
                    arrowPos.y = arrowPos.y + targetBounds.size.y / 2 + arrowBounds.size.y / 2 + pOffset;
                    arrowPos.x = arrowPos.x + targetBounds.size.x / 2 + arrowBounds.size.x / 2 + pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, -45));
                    break;
                case Alignment.Left:
                    arrowPos.x = arrowPos.x - targetBounds.size.x / 2 - arrowBounds.size.x / 2 + pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, 90));
                    break;
                case Alignment.Center:
                    break;
                case Alignment.Right:
                    arrowPos.x = arrowPos.x + targetBounds.size.x / 2 + arrowBounds.size.x / 2 + pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, -90));
                    break;
                case Alignment.BotLeft:
                    arrowPos.y = arrowPos.y - targetBounds.size.y / 2 - arrowBounds.size.y / 2 - pOffset;
                    arrowPos.x = arrowPos.x - targetBounds.size.x / 2 - arrowBounds.size.x / 2 - pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, -235));
                    break;
                case Alignment.Bot:
                    arrowPos.y = arrowPos.y - targetBounds.size.y / 2 - arrowBounds.size.y / 2 - pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, 180));
                    break;
                case Alignment.BotRight:
                    arrowPos.y = arrowPos.y - targetBounds.size.y / 2 - arrowBounds.size.y / 2 - pOffset;
                    arrowPos.x = arrowPos.x + targetBounds.size.x / 2 + arrowBounds.size.x / 2 + pOffset;
                    mRectPointer.eulerAngles = (new Vector3(0, 0, 235));
                    break;
            }

            mRectPointer.anchoredPosition = arrowPos;
            enabled = true;

            if (pPostValidate)
                CoroutineUtil.StartCoroutine(IEPostValidatingPointer(pTarget, pAlignment, pOffset));
        }

        public void MessageToTarget(RectTransform pTarget, string pMessage, Alignment pAlignment, Vector2 pSize, float pOffset = 30, bool pPostValidate = true)
        {
            mRectMessage.SetActive(true);
            mTxtMessage.text = pMessage;
            mRectMessage.sizeDelta = pSize;

            if (pSize.x < 300 || pSize.y < 160)
            {
                mDialogGroup.SetActive(false);
                mTxtMessage.alignment = TextAlignmentOptions.Center;
            }
            else
            {
                mDialogGroup.SetActive(true);
                mTxtMessage.alignment = TextAlignmentOptions.TopLeft;
            }

            if (pTarget == null)
                mRectMessage.anchoredPosition = Vector2.zero;
            else
            {
                mRectMessage.position = pTarget.position;
                var targetPivot = pTarget.pivot;
                var x = mRectMessage.anchoredPosition.x - pTarget.rect.width * targetPivot.x + pTarget.rect.width / 2f;
                var y = mRectMessage.anchoredPosition.y - pTarget.rect.height * targetPivot.y + pTarget.rect.height / 2f;
                mRectMessage.anchoredPosition = new Vector2(x, y);

                var targetBounds = pTarget.Bounds();
                var boxBounds = mRectMessage.Bounds();
                var messageBoxPos = mRectMessage.anchoredPosition;

                switch (pAlignment)
                {
                    case Alignment.TopLeft:
                        messageBoxPos.y = messageBoxPos.y + targetBounds.size.y / 2 + boxBounds.size.y / 2 + pOffset;
                        messageBoxPos.x = messageBoxPos.x - targetBounds.size.x / 2 - boxBounds.size.x / 2 - pOffset;
                        break;
                    case Alignment.Top:
                        messageBoxPos.y = messageBoxPos.y + targetBounds.size.y / 2 + boxBounds.size.y / 2 + pOffset;
                        break;
                    case Alignment.TopRight:
                        messageBoxPos.y = messageBoxPos.y + targetBounds.size.y / 2 + boxBounds.size.y / 2 + pOffset;
                        messageBoxPos.x = messageBoxPos.x + targetBounds.size.x / 2 + boxBounds.size.x / 2 + pOffset;
                        break;
                    case Alignment.Left:
                        messageBoxPos.x = messageBoxPos.x - targetBounds.size.x / 2 - boxBounds.size.x / 2 - pOffset;
                        break;
                    case Alignment.Center:
                        break;
                    case Alignment.Right:
                        messageBoxPos.x = messageBoxPos.x + targetBounds.size.x / 2 + boxBounds.size.x / 2 + pOffset;
                        break;
                    case Alignment.BotLeft:
                        messageBoxPos.y = messageBoxPos.y - targetBounds.size.y / 2 - boxBounds.size.y / 2 - pOffset;
                        messageBoxPos.x = messageBoxPos.x - targetBounds.size.x / 2 - boxBounds.size.x / 2 - pOffset;
                        break;
                    case Alignment.Bot:
                        messageBoxPos.y = messageBoxPos.y - targetBounds.size.y / 2 - boxBounds.size.y / 2 - pOffset;
                        break;
                    case Alignment.BotRight:
                        messageBoxPos.y = messageBoxPos.y - targetBounds.size.y / 2 - boxBounds.size.y / 2 - pOffset;
                        messageBoxPos.x = messageBoxPos.x + targetBounds.size.x / 2 + boxBounds.size.x / 2 + pOffset;
                        break;
                }
                mRectMessage.anchoredPosition = messageBoxPos;
            }
            enabled = true;

            if (pPostValidate)
                CoroutineUtil.StartCoroutine(IEPostValidatingMessage(pTarget, pMessage, pAlignment, pSize, pOffset));
        }

        private IEnumerator IEPostValidatingPointer(RectTransform pTarget, Alignment pAlignment, float pOffset = 0)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return null;
                PointToTarget(pTarget, pAlignment, pOffset, false);
            }
        }

        private IEnumerator IEPostValidatingMessage(RectTransform pTarget, string pMessage, Alignment pAlignment, Vector2 pSize, float pOffset = 30)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return null;
                MessageToTarget(pTarget, pMessage, pAlignment, pSize, pOffset, false);
            }
        }
    }
}