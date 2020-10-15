using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToTarget : MonoBehaviour
{
    public Transform objectTap;
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
#if UNITY_EDITOR
        if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject(-1))    // is the touch on the GUI
        {
            // GUI Action
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = GameplayController.Instance.camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == objectTap)
                {
                    var pos = GameplayController.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
                    target.position = new Vector3(pos.x, pos.y, target.position.z);
                    GameplayController.Instance.SetHeroesLookAt(target);
                }
            }
        }
#else
        if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject(0))    // is the touch on the GUI
        {
            // GUI Action
            return;
        }

        if (Input.touchCount > 0)
        {
            foreach (var item in Input.touches)
            {
                if (item.phase == TouchPhase.Began
                    || item.phase == TouchPhase.Moved) {
                    Ray ray = GameplayController.Instance.camera.ScreenPointToRay(item.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == objectTap)
                        {
                            var pos = GameplayController.Instance.camera.ScreenToWorldPoint(item.position);
                            target.position = new Vector3(pos.x, pos.y, target.position.z);
                            GameplayController.Instance.SetHeroesLookAt(target);
                        }
                    }
                }
            }
        }
#endif
    }
}
