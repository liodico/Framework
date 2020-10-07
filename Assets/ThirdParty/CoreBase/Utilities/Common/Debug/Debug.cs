using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace Utilities.Common
{
    public static class Debug
    {
        public static void Log(object message)
        {
            if (!DevSetting.Instance.enableLog) return;
            UnityEngine.Debug.Log(message);
        }

        public static void Log(object message, Color pColor = default(Color))
        {
            if (!DevSetting.Instance.enableLog) return;
            string c = pColor == default(Color) ? ColorUtility.ToHtmlStringRGBA(Color.yellow) : ColorUtility.ToHtmlStringRGBA(pColor);
            UnityEngine.Debug.Log("<color=\"#" + c + "\">" + message + "</color>");
        }

        public static void Log(object title, object message, Color pColor = default(Color))
        {
            if (!DevSetting.Instance.enableLog) return;
            string c = pColor == default(Color) ? ColorUtility.ToHtmlStringRGBA(Color.yellow) : ColorUtility.ToHtmlStringRGBA(pColor);
            UnityEngine.Debug.Log("<b><color=\"#" + c + "\">" + title + "</color></b> " + message);
        }

        public static void LogError(object message)
        {
            if (!DevSetting.Instance.enableLog) return;
            UnityEngine.Debug.LogError(message);
        }

        public static void LogWarning(object message)
        {
            if (!DevSetting.Instance.enableLog) return;
            UnityEngine.Debug.LogWarning(message.ToString());
        }

        public static void LogException(System.Exception e)
        {
            if (!DevSetting.Instance.enableLog) return;
            UnityEngine.Debug.LogException(e);
        }

        public static void LogJson<T>(T pObj, Color pColor = default(Color)) where T : class
        {
            if (!DevSetting.Instance.enableLog) return;

            if (pObj == null)
                return;

            var jsonStr = JsonUtility.ToJson(pObj);
            if (jsonStr != "{}")
            {
                string c = pColor == default(Color) ? ColorUtility.ToHtmlStringRGBA(Color.yellow) : ColorUtility.ToHtmlStringRGBA(pColor);
                UnityEngine.Debug.Log(string.Format("<color=\"#{2}\">{0}</color>{1}", pObj.GetType().FullName, jsonStr, c));
            }
        }

        public static void LogNewtonJson(object pObj, Color pColor = default(Color))
        {
            if (!DevSetting.Instance.enableLog)
                return;
            /*
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(pObj);
            if (jsonStr != "{}")
            {
                string c = pColor == default(Color) ? ColorUtility.ToHtmlStringRGBA(Color.yellow) : ColorUtility.ToHtmlStringRGBA(pColor);
                UnityEngine.Debug.Log(string.Format("<color=\"#{2}\">{0}</color>{1}", pObj.GetType().FullName, jsonStr, c));
            }
            */
        }

        /// <summary>
        /// Print out an array.
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="array">Array to print.</param>
        /// <param name="additionalText">Additional text to print.</param>
        public static void LogArray<T>(T[] array, string additionalText = "")
        {
            if (!DevSetting.Instance.enableLog) return;

            string s = additionalText + "[";
            for (int i = 0; i < array.Length; i++)
            {
                s += (array[i] != null ? array[i].ToString() : "null") + (i < array.Length - 1 ? ", " : "");
            }
            s += "]";
            UnityEngine.Debug.Log(s);
        }

        /// <summary>
        /// Print a dictionary.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="dict">Dictionary to print</param>
        /// <param name="additionalText">Additional text to print before dict</param>
        public static void LogDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, string additionalText = "")
        {
            if (!DevSetting.Instance.enableLog) return;

            string log = "";
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                log += "[" + pair.Key.ToString() + ": " + pair.Value.ToString() + "]";
            }
            UnityEngine.Debug.Log(additionalText + log);
        }

        /// <summary>
        /// Print a List.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="message">Massage before list.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void LogList<T>(List<T> list, string message = "")
        {
            if (!DevSetting.Instance.enableLog) return;

            string log = "{";
            for (int i = 0; i < list.Count; i++)
            {
                log += list[i].ToString() + "| ";
            }
            log += "}";
            if (!string.IsNullOrEmpty(message))
                UnityEngine.Debug.Log(message + log);
        }

        /// <summary>
        /// Print a log to check if objects are null or not.
        /// </summary>
        /// <param name="objs">Objects to check.</param>
        public static void LogNull(params object[] objs)
        {
            if (!DevSetting.Instance.enableLog) return;

            string log = "";
            for (int i = 0; i < objs.Length; i++)
            {
                log += i + " is " + (objs[i] == null ? "" : "NOT ") + "NULL";
                if (i < objs.Length - 1)
                    log += ", ";
            }
            UnityEngine.Debug.Log(log);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogToFile(object context, string fileName, bool deleteAfterQuit = true)
        {
            if (DevSetting.Instance.enableLog && Application.isEditor)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, true);
                sw.WriteLine(string.Format("{0:0.00} \t {1} \t {2}", Time.time, Time.frameCount, context));
                sw.Close();
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void WriteJsonObject(object pObj, string pFileName)
        {
            File.WriteAllText("D:\\" + pFileName + ".txt", JsonUtility.ToJson(pObj));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void WriteFile(string pContent, string pFileName)
        {
            File.WriteAllText("D:\\" + pFileName + ".txt", pContent);
        }

        public static void Assert(bool pCondition, string pLog)
        {
            if (!DevSetting.Instance.enableLog) return;
            UnityEngine.Debug.Assert(pCondition, pLog);
        }
    }
}