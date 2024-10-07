using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatLevelUpButton : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound("buttonHover");
    }
}
