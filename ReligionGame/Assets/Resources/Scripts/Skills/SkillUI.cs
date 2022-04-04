using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IDropHandler
{
    [HideInInspector] public Image SkillIcon {
        get {
            if (SkillIcon.sprite != null)
            {
                return SkillIcon;
            }
            return null;
        }
    }

    [HideInInspector] public Text SkillCastLetter;

    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (!SkillIcon)
        {
            SkillDragHandler.itemBeingDragged.transform.SetParent(transform);
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
    #endregion

    public void Start()
    {
        //SkillIcon = GetComponent<Image>();
        SkillCastLetter = GetComponentInChildren<Text>();
    }
}

