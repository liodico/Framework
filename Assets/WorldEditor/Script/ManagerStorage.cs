using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WorldEditor
{
    public static class ManagerStorage
    {
        public static string LoadFile(string pPath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(pPath);
            if (textAsset != null)
            {
                string content = "";
                content = textAsset.text;
                Resources.UnloadAsset(textAsset);
                return content;
            }
            return "";
        }
    }
}