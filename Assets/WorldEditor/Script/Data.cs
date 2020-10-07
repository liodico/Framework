using System;
using System.Collections.Generic;

namespace WorldEditor
{
    [Serializable]
    public class Object
    {
        public int type;
        public int id;
        public float timeAppear;
        public float timeIdle;
        public float x;
        public float y;

        public Object Clone()
        {
            var clone = new Object();
            clone.type = type;
            clone.id = id;
            clone.timeAppear = timeAppear;
            clone.timeIdle = timeIdle;
            clone.x = x;
            clone.y = y;
            return clone;
        }

        public string GetJson()
        {
            var str = "\"type\":" + type + ",";
            str += "\"id\":" + id + ",";
            str += "\"timeAppear\":" + timeAppear + ",";
            str += "\"timeIdle\":" + timeIdle + ",";
            str += "\"x\":" + x + ",";
            str += "\"y\":" + y;
            return str;
        }
    }
    [Serializable]
    public class Wave
    {
        public List<Object> listObj;
        public Wave()
        {
            listObj = new List<Object>();
        }
    }
    [Serializable]
    public class Level
    {
        public List<Wave> listWave;

        public Level()
        {
            listWave = new List<Wave>();
            listWave.Add(new Wave());
        }
    }
}