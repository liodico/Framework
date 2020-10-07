using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utilities.Common;
using Utilities.Components;
using System;


namespace FoodZombie.UI
{
    public class GiftCodePanel : MyGamesBasePanel
    {
        #region Members

        [SerializeField] private TMP_InputField mInputFieldCode;
        [SerializeField] private TextMeshProUGUI mTxtMessage;
        [SerializeField] private JustButton mBtnSubmit;
        [SerializeField] private JustButton mBtnPaste;

        private bool mIsProcessing;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            mInputFieldCode.onValueChanged.AddListener(OnInputCode_Changed);
            mBtnSubmit.onClick.AddListener(OnBtnSubmit_Pressed);
            mBtnPaste.onClick.AddListener(OnBtnPaste_Pressed);
        }

        private void OnEnable()
        {
            mInputFieldCode.text = "";
            mTxtMessage.text = "";

            if (!ServerManager.Authorized)
            {
                mInputFieldCode.placeholder.SetActive(false);
                mTxtMessage.text = "No Connection";
                mBtnSubmit.SetEnable(false);
                mInputFieldCode.enabled = false;
                mBtnPaste.SetEnable(false);
            }
            else
            {
                mInputFieldCode.placeholder.SetActive(true);
                mBtnSubmit.SetEnable(true);
                mInputFieldCode.enabled = true;
                mBtnPaste.SetEnable(true);
            }
        }

        #endregion

        private void OnInputCode_Changed(string pValue)
        {
            mInputFieldCode.placeholder.SetActive(string.IsNullOrEmpty(mTxtMessage.text));
            mTxtMessage.text = "";
        }

        private void OnBtnPaste_Pressed()
        {
            mInputFieldCode.text = UniClipboard.GetText();
        }

        private void OnBtnSubmit_Pressed()
        {
            if (string.IsNullOrEmpty(mInputFieldCode.text.Trim()))
            {
                mInputFieldCode.text = "";
                mInputFieldCode.placeholder.SetActive(false);
                mTxtMessage.text = "You must fill out gift code!";
                return;
            }

            ServerManager.ClaimGiftCode(mInputFieldCode.text, (reward, messageCode) =>
            {
                if (reward != null)
                {
                    reward = LogicAPI.ClaimReward(reward, true);
                    if (reward != null)
                    {
                        MainPanel.instance.ShowRewardsPopup(reward);
                    }
                    else
                    {
                        mInputFieldCode.text = "";
                        mTxtMessage.text = "Code is invalid";
                        mInputFieldCode.placeholder.SetActive(false);
                    }
                }
                else
                {
                    mInputFieldCode.text = "";
                    switch (messageCode)
                    {
                        case "used":
                            mTxtMessage.text = "Code is already used";
                            break;
                        case "not_found":
                            mTxtMessage.text = "Code is invalid";
                            break;
                        case "expired":
                            mTxtMessage.text = "Code has expired";
                            break;
                        default:
                            mTxtMessage.text = "Connection Error";
                            break;
                    }
                    mInputFieldCode.placeholder.SetActive(false);
                }
            });
        }
    }
}