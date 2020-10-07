/***
 * Created by HungNB - NBEAR
 * 
 * Usage:
 * YouObject[] objects = JsonHelper.GetJsonArray<YouObject> (jsonString);
 * string jsonString = JsonHelper.ArrayToJson<YouObject>(objects);
 **/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WorldEditor
{
    public static class JsonHelper
    {
        public static T[] GetJsonArray<T>(string json)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string newJson = sb.Append("{").Append("\"array\":").Append(json).Append("}").ToString();
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
                return wrapper.array;
            }
            catch
            {
                Debug.LogError((typeof(T).Name));
                return null;
            }
        }

        public static List<T> GetJsonList<T>(string json)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string newJson = sb.Append("{").Append("\"list\":").Append(json).Append("}").ToString();
                ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(newJson);
                return wrapper.list;
            }
            catch
            {
                Debug.LogError((typeof(T).Name));
                return null;
            }
        }

        public static string ArrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>
            {
                array = array
            };
            string json = JsonUtility.ToJson(wrapper);
            json = json.Remove(0, 9);
            json = json.Remove(json.Length - 1);
            return json;
        }

        public static string ListToJson<T>(List<T> list)
        {
            ListWrapper<T> wrapper = new ListWrapper<T>
            {
                list = list
            };
            string json = JsonUtility.ToJson(wrapper);
            json = json.Remove(0, 8);
            json = json.Remove(json.Length - 1);
            return json;
        }

        [Serializable]
        class Wrapper<T>
        {
            public T[] array;
        }

        [Serializable]
        class ListWrapper<T>
        {
            public List<T> list;
        }
    }
}