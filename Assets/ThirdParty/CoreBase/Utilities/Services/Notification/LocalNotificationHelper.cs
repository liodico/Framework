using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities.Service.Notification
{
    /// <summary>
    /// NOTE: Only work with Mobile NOtifications version 1.0.4
    /// </summary>

    [RequireComponent(typeof(GameNotificationsManager))]
    public class LocalNotificationHelper : MonoBehaviour
    {
        #region Members

        private static LocalNotificationHelper mInstance;
        public static LocalNotificationHelper Instance => mInstance;

        public const string CHANNEL_ID = "game_channel_0";

        [SerializeField] private GameNotificationsManager mManager;

        #endregion

        //=====================================

        #region MonoBehaviour

        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
#if UNITY_NOTIFICATION
            mManager.LocalNotificationDelivered += OnLocalNotificationDelivered;
            mManager.LocalNotificationExpired += OnLocalNotificationExpired;

            var channel = new GameNotificationChannel(CHANNEL_ID, "Default Game Channel", "Generic Notification");

            mManager.Initialize(channel);

            CancelAllNotifications();
#endif
        }

        private void OnDestroy()
        {
#if UNITY_NOTIFICATION
            mManager.LocalNotificationDelivered -= OnLocalNotificationDelivered;
            mManager.LocalNotificationExpired -= OnLocalNotificationExpired;
#endif
        }

        //private void OnApplicationQuit()
        //{
        //    DateTime date = DateTime.Now;
        //    TimeSpan time = new TimeSpan(0, 0, 0, 30);
        //    DateTime combined = date.Add(time);
        //    SendNotification("", "The zombies are too crowded and aggressive. Help me kill them!", combined);
        //}

        //public void OnApplicationPause(bool pPaused)
        //{
        //    if (pPaused)
        //    {
        //        DateTime date = DateTime.Now;
        //        TimeSpan time = new TimeSpan(0, 0, 0, 30);
        //        DateTime combined = date.Add(time);
        //        SendNotification("", "The zombies are too crowded and aggressive. Help me kill them!", combined);
        //    }
        //}

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (mManager == null)
                mManager = GetComponent<GameNotificationsManager>();
        }
#endif

        #endregion

        //=====================================

        #region Public

        /// <summary>
        /// Queue a notification with the given parameters.
        /// </summary>
        /// <param name="pTitle">The title for the notification.</param>
        /// <param name="pBody">The body text for the notification.</param>
        /// <param name="pDeliveryTime">The time to deliver the notification.</param>
        /// <param name="pBadgeNumber">The optional badge number to display on the application icon.</param>
        /// <param name="pReschedule">Whether to reschedule the notification if foregrounding and the notification hasn't yet been shown.</param>
        /// <param name="pChannelId">Channel ID to use. If this is null/empty then it will use the default ID. For Android the channel must be registered in <see cref="GameNotificationsManager.Initialize"/>.</param>
        /// <param name="pSmallIcon">Notification small icon.</param>
        /// <param name="pLargleIcon">Notification large icon.</param>
        public int SendNotification(string pTitle, string pBody, DateTime pDeliveryTime, int? pBadgeNumber = null,
            bool pReschedule = false, string pChannelId = null, string pSmallIcon = null, string pLargleIcon = null)
        {
#if UNITY_NOTIFICATION
            if (!mManager.Initialized)
                return -1;

            IGameNotification notification = mManager.CreateNotification();

            if (notification == null)
                return -1;

            notification.Title = pTitle;
            notification.Body = pBody;
            notification.Group = !string.IsNullOrEmpty(pChannelId) ? pChannelId : CHANNEL_ID;
            notification.DeliveryTime = pDeliveryTime;
            notification.SmallIcon = pSmallIcon;
            notification.LargeIcon = pLargleIcon;
            if (pBadgeNumber != null)
                notification.BadgeNumber = pBadgeNumber;

            PendingNotification notificationToDisplay = mManager.ScheduleNotification(notification);
            notificationToDisplay.Reschedule = pReschedule;

#if DEVELOPMENT || UNITY_EDITOR
            UnityEngine.Debug.Log($"Queued event with ID \"{notification.Id}\" at time {pDeliveryTime:HH:mm}");
#endif
            return notification.Id.Value;
#endif
            return -1;
        }

        public void CancelNotification(PendingNotification pObj)
        {
#if UNITY_NOTIFICATION
            mManager.CancelNotification(pObj.Notification.Id.Value);

#if DEVELOPMENT || UNITY_EDITOR
            UnityEngine.Debug.Log($"Cancelled notification with ID \"{pObj.Notification.Id}\"");
#endif
#endif
        }

        public void CancelAllNotifications()
        {
#if UNITY_NOTIFICATION
            mManager.CancelAllNotifications();
#endif
        }

        public void CancelNotification(int pId)
        {
#if UNITY_NOTIFICATION
            mManager.CancelNotification(pId);
#endif
        }

        #endregion

        //=====================================

        #region Private

#if UNITY_NOTIFICATION
        private void OnLocalNotificationExpired(PendingNotification pObj)
        {
#if DEVELOPMENT || UNITY_EDITOR
            UnityEngine.Debug.Log($"Notification ID \"{pObj.Notification.Id}\" Title \"{pObj.Notification.Title}\" expired and was not displayed.");
#endif
        }

        private void OnLocalNotificationDelivered(PendingNotification pObj)
        {
#if DEVELOPMENT || UNITY_EDITOR
            UnityEngine.Debug.Log($"Notification ID \"{pObj.Notification.Id} Title \"{pObj.Notification.Title}\" shown in foreground.");
#endif
        }
#endif

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LocalNotificationHelper))]
    public class LocalNotificationHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorHelper.BoxVertical(() =>
            {

#if UNITY_NOTIFICATION
                if (EditorHelper.ButtonColor("Disable Local notification", Color.red))
                    EditorHelper.RemoveDirective("UNITY_NOTIFICATION");
#else
                EditorGUILayout.HelpBox("Note: Must Install Mobile Notification from Package Manager", MessageType.Info);

                if (EditorHelper.ButtonColor("Enable Local notification", Color.green))
                    EditorHelper.AddDirective("UNITY_NOTIFICATION");
#endif
            }, Color.white, true);
        }
    }
#endif
}