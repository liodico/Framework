using System;
using UnityEngine;
using TMPro;

namespace WorldEditor
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] WorldEditor world;
        [SerializeField] EasyButton btnBack;
        [SerializeField] TextMeshProUGUI txtLevel;
        [SerializeField] TMP_InputField ebLevel;
        [SerializeField] EasyButton btnGo;
        [SerializeField] EasyButton btnClone;
        [SerializeField] EasyButton btnAdd;
        [SerializeField] EasyButton btnDelete;

        void Start()
        {
            btnBack.OnClick(OnClickShow);
            btnGo.OnClick(OnClickGo);
            btnClone.OnClick(OnClickClone);
            btnAdd.OnClick(OnClickAdd);
            btnDelete.OnClick(OnClickDelete);
        }

        public void OnClickShow()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            txtLevel.text = "LEVEL (MAX " + world.listLevel.Count + ")";
        }

        void OnClickGo()
        {
            int num;
            if (!int.TryParse(ebLevel.text, out num)) return;
            if (num < 1 || num > world.listLevel.Count) return;
            world.LoadLevel(num - 1);
        }

        void OnClickClone()
        {
            // int num;
            // if (!int.TryParse(ebLevel.text, out num)) return;
            // if (num <= 0 || num > world.listLevel.Count) return;
            // world.listLevel[world.currentLevel - 1] = world.listLevel[num - 1];
            // world.LoadLevel();
        }

        void OnClickAdd()
        {
            world.listLevel.Add(null);
            for (int i = world.listLevel.Count - 1; i > world.currentLevel + 1; --i)
                world.listLevel[i] = world.listLevel[i - 1];
            world.listLevel[world.currentLevel + 1] = new Level();
            ebLevel.text = "" + world.listLevel.Count;
            txtLevel.text = "LEVEL (MAX " + world.listLevel.Count + ")";
            world.LoadLevel(world.currentLevel + 1);
        }

        void OnClickDelete()
        {
            if (world.listLevel.Count < 2) return;
            world.listLevel.RemoveAt(world.currentLevel);
            if (world.currentLevel >= world.listLevel.Count) --world.currentLevel;
            txtLevel.text = "LEVEL (MAX " + world.listLevel.Count + ")";
            world.LoadLevel(world.currentLevel);
        }
    }
}