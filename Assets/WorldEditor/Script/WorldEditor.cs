using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using TMPro;

namespace WorldEditor
{
    public class WorldEditor : MonoBehaviour
    {
        public string loadPath;

        [SerializeField] List<ObjectPrefab> listPrefab;
        [SerializeField] EasyButton btnLevel;
        [SerializeField] LevelManager levelManager;

        [NonSerialized] public List<Level> listLevel;
        [NonSerialized] public int currentLevel;
        [SerializeField] TextMeshProUGUI txtLevel;

        [SerializeField] EasyButton btnPrevWave;
        [SerializeField] EasyButton btnNextWave;
        [SerializeField] EasyButton btnDeleteWave;
        [SerializeField] EasyButton btnCopyWave;
        [SerializeField] EasyButton btnPasteWave;
        [SerializeField] EasyButton btnAddWave;

        [SerializeField] TextMeshProUGUI txtWave;
        [NonSerialized] public int currentWave;
        Wave waveCopied;

        [SerializeField] ObjectData objectData;
        [SerializeField] EasyButton btnAdd;
        [SerializeField] GameObject nAdd;
        [SerializeField] EasyButton btnBack;
        [SerializeField] List<ObjectSlot> objSlot;

        [SerializeField] EasyButton btnSave;

        void Start()
        {
            if (File.Exists(loadPath))
            {
                var stream = File.OpenText(loadPath);
                var str = stream.ReadToEnd();
                stream.Close();
                listLevel = JsonHelper.GetJsonList<Level>(str);
            }

            if (listLevel is null)
            {
                listLevel = new List<Level>();
                listLevel.Add(new Level());
            }

            objSlot.Refresh();
            for (int i = 0; i < objectData.arrObject.Length; ++i)
            {
                var slot = objSlot.GetClone();
                slot.Set(ObjectData.TYPE_OBJECT, i);
            }
            for (int i = 0; i < objectData.arrUnit.Length; ++i)
            {
                var slot = objSlot.GetClone();
                slot.Set(ObjectData.TYPE_UNIT, i);
                slot.button.OnClick(() => { OnClickAdd(slot.id, slot.type); });
            }

            btnLevel.OnClick(levelManager.OnClickShow);
            btnPrevWave.OnClick(OnClickPrevWave);
            btnNextWave.OnClick(OnClickNextWave);
            btnDeleteWave.OnClick(OnClickDeleteWave);
            btnCopyWave.OnClick(OnClickCopyWave);
            btnPasteWave.OnClick(OnClickPasteWave);
            btnAddWave.OnClick(OnClickAddWave);

            btnAdd.OnClick(OnClickShowAdd);
            btnBack.OnClick(OnClickShowAdd);

            LoadLevel(0, false);

            btnSave.OnClick(OnClickSave);
        }

        public void LoadLevel(int level, bool save = true)
        {
            if (save) SaveWave();
            currentLevel = level;
            currentWave = 0;
            txtLevel.text = "LEVEL " + (currentLevel + 1);
            LoadWave(currentWave, false);
        }

        void SaveWave()
        {
            var listObject = listLevel[currentLevel].listWave[currentWave].listObj;
            listObject.Clear();
            foreach (var obj in listPrefab)
            {
                if (obj.gameObject.activeSelf)
                {
                    var newObj = new Object();
                    newObj.id = obj.id;
                    newObj.type = obj.type;
                    newObj.x = obj.transform.localPosition.x;
                    newObj.y = obj.transform.localPosition.y;
                    newObj.timeAppear = float.Parse(obj.ebTimeAppear.text);
                    newObj.timeIdle = float.Parse(obj.ebTimeIdle.text);
                    listObject.Add(newObj);
                }
            }
        }

        public void LoadWave(int wave, bool save = true)
        {
            if (save) SaveWave();
            currentWave = wave;
            txtWave.text = "WAVE " + (currentWave + 1);
            listPrefab.Refresh();
            var listObject = listLevel[currentLevel].listWave[currentWave].listObj;
            foreach (var obj in listObject)
            {
                var prefab = listPrefab.GetClone();
                prefab.transform.localPosition = new Vector3(obj.x, obj.y, 0);
                prefab.Set(obj.type, obj.id, obj.timeAppear, obj.timeIdle);
            }
        }

        void OnClickPrevWave()
        {
            if (currentWave < 1) return;
            LoadWave(currentWave - 1);
        }

        void OnClickNextWave()
        {
            if (currentWave >= listLevel[currentLevel].listWave.Count - 1) return;
            LoadWave(currentWave + 1);
        }

        void OnClickDeleteWave()
        {
            listLevel[currentLevel].listWave.RemoveAt(currentWave);
            if (listLevel[currentLevel].listWave.Count == 0)
            {
                listLevel[currentLevel].listWave.Add(new Wave());
                LoadWave(0);
            }
            else LoadWave(currentWave - 1);
        }

        void OnClickCopyWave()
        {
            waveCopied = listLevel[currentLevel].listWave[currentWave];
        }

        void OnClickPasteWave()
        {
            listLevel[currentLevel].listWave[currentWave].listObj.Clear();
            foreach (var obj in waveCopied.listObj)
                listLevel[currentLevel].listWave[currentWave].listObj.Add(obj.Clone());
            LoadWave(currentWave);
        }

        void OnClickAddWave()
        {
            var level = listLevel[currentLevel];
            level.listWave.Add(new Wave());
            LoadWave(level.listWave.Count - 1);
        }

        void OnClickShowAdd()
        {
            nAdd.SetActive(!nAdd.activeSelf);
        }

        void OnClickAdd(int id, int type)
        {
            var obj = listPrefab.GetClone();
            obj.Set(type, id);
            obj.transform.position = Vector3.zero;
        }

        void OnClickSave()
        {
            SaveWave();
            string str = "[";
            for (int i = 0; i < listLevel.Count; ++i)
            {
                str += "{\"listWave\":[";
                for (int j = 0; j < listLevel[i].listWave.Count; ++j)
                {
                    str += "{\"listObj\":[";
                    for (int k = 0; k < listLevel[i].listWave[j].listObj.Count; ++k)
                    {
                        str += "{";
                        str += listLevel[i].listWave[j].listObj[k].GetJson();
                        str += "}";
                        if (k != listLevel[i].listWave[j].listObj.Count - 1) str += ",";
                    }
                    str += "]}";
                    if (j != listLevel[i].listWave.Count - 1) str += ",";
                }
                str += "]}";
                if (i != listLevel.Count - 1) str += ",";
            }
            str += "]";

            var stream = File.Open(loadPath, FileMode.OpenOrCreate);
            var bytes = new UTF8Encoding(true).GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            Debug.Log(loadPath + " was created");
        }

#if UNITY_EDITOR
        // [CustomEditor(typeof(WorldEditor))]
        // public class AssetsCollectionEditor : Editor
        // {
        //     WorldEditor worldEditor;
        //     void OnEnable()
        //     {
        //         worldEditor = (WorldEditor)target;
        //     }
        //     public override void OnInspectorGUI()
        //     {
        //         base.OnInspectorGUI();
        //         if (GUILayout.Button("Export"))
        //         {
        //             var list = new List<ObjectPos>();
        //             foreach (var node in worldEditor.Waves)
        //             {
        //                 for (int i = 0; i < node.transform.childCount; ++i)
        //                     list.Add(node.transform.GetChild(i).GetComponent<ObjectPos>());
        //             }
        //             var str = worldEditor.GetJson();
        //             for (int i = 0; i < list.Count; ++i)
        //             {
        //                 if (i > 0) str += ",";
        //                 str += list[i].GetJson();
        //             }
        //             str += "]}";

        //             var path = worldEditor.path + worldEditor.worldName + ".txt";
        //             if (File.Exists(path)) File.Delete(path);
        //             var stream = File.Create("D:/" + worldEditor.worldName + ".txt");
        //             var bytes = new UTF8Encoding(true).GetBytes(str);
        //             stream.Write(bytes, 0, bytes.Length);
        //             stream.Close();
        //             Debug.Log(path + " was created");
        //         }
        //     }
        // }
#endif
    }
}