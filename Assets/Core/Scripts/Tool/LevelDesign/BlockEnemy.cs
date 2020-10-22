using FoodZombie;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class BlockEnemy : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI txtLevel;
    private int index;
    private UnityAction<int> addAction;
    private UnityAction<int> removeAction;

    public void Init(int _index, EnemyInfo _enemyInfo, UnityAction<int> _addAction, UnityAction<int> _removeAction)
    {
        index = _index;
        addAction = _addAction;
        removeAction = _removeAction;

        if (_enemyInfo.id != 0)
        {
            icon.sprite = AssetsCollection.instance.enemyIcon.GetAsset(_enemyInfo.id - 1);
            txtLevel.text = _enemyInfo.level + "";
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            addAction(index);
        }
        // else if (eventData.button == PointerEventData.InputButton.Middle)
        //     Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            removeAction(index);
        }
    }
}