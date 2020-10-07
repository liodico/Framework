using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldEditor
{
    public static class ManagerCode
    {
        public static Vector3 GetLpos(this GameObject obj)
        {
            return obj.transform.localPosition;
        }
        public static void SetLpos(this GameObject obj, Vector3 v)
        {
            obj.transform.localPosition = v;
        }
        public static void SetLposX(this GameObject obj, float v)
        {
            obj.transform.localPosition = new Vector3(v, obj.transform.localPosition.y, obj.transform.localPosition.z);
        }
        public static void SetLposY(this GameObject obj, float v)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, v, obj.transform.localPosition.z);
        }
        public static string ConvertMoney(this int value, bool KM = false, bool change = false)
        {
            string first = change ? (value < 0 ? "+" : "-") : "";
            string last = "";
            value = Mathf.Abs(value);

            if (KM)
            {
                var str = "" + value;
                if (value > 999999999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 9));
                    last = "," + str.Substring(str.Length - 9, 1) + "B";
                }
                else if (value > 999999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 6));
                    last = "," + str.Substring(str.Length - 6, 1) + "M";
                }
                else if (value > 999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 3));
                    last = "," + str.Substring(str.Length - 3, 1) + "K";
                }
            }

            var begin = "" + value;
            var result = "";
            while (begin.Length > 3)
            {
                result = "." + begin.Substring(begin.Length - 3, 3) + result;
                begin = begin.Substring(0, begin.Length - 3);
            }
            return first + begin + result + last;
        }

        public static int GetMoneyValue(this string str)
        {
            str = str.Replace(".", "");
            str = str.Replace(",", "");
            str = str.Replace(" ", "");
            str = str.Replace("+", "");
            str = str.Replace("-", "");
            return int.Parse(str);
        }

        public static string StringShorten(this string value, int len)
        {
            var str = "" + value;
            return (str.Length > len ? str.Substring(0, len - 1) + "..." : str);
        }

        public static string GetTime(this int value)
        {
            var min = Mathf.Floor(value / 60);
            var second = value - min * 60;
            var str = "";
            if (min > 9) str = "" + min;
            else str = "0" + min;
            return (second > 9 ? str + ":" + second : str + ":0" + second);
        }

        public static void Refresh<T>(this List<T> list) where T : Component
        {
            foreach (var data in list)
            {
                data.gameObject.SetActive(false);
            }
        }

        public static T GetClone<T>(this List<T> list, GameObject parent = null) where T : Component
        {
            if (parent is null) parent = list[0].gameObject.transform.parent.gameObject;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].gameObject.activeSelf)
                {
                    list[i].transform.SetParent(parent.transform);
                    list[i].gameObject.SetActive(true);
                    return list[i];
                }
            }

            T obj = UnityEngine.Object.Instantiate(list[0]);
            list.Add(obj);
            obj.transform.SetParent(parent.transform);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public static List<int> GetRandomListFair(List<float> list, int count)
        {
            List<int> arrT = new List<int>();
            for (int i = 0; i < list.Count; ++i)
            {
                while (list[i] >= 1)
                {
                    arrT.Add(i);
                    --list[i];
                }
            }

            while (arrT.Count < count)
            {
                int t = GetRandomWithUnlimitedChance(list);
                list[t] -= 1;
                if (list[t] < 0) list[t] = 0;
                arrT.Add(t);
            }

            List<int> result = new List<int>();
            while (arrT.Count > 0)
            {
                int r = UnityEngine.Random.Range(0, arrT.Count);
                result.Add(arrT[r]);
                arrT.RemoveAt(r);
            }
            return result;
        }

        public static int GetRandomWithUnlimitedChance(List<float> list)
        {
            float s = 0;
            foreach (float i in list) s += i;

            float r = UnityEngine.Random.Range(0, s);
            int t = 0;
            s = list[t];
            while (r >= s)
            {
                ++t;
                s += list[t];
            }
            return t;
        }
    }
}
