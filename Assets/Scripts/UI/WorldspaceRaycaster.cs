using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldspaceRaycaster : GraphicRaycaster
{
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        //Set middle screen pos or you can set variable on start and use it
        eventData.position = new(Screen.width / 2f, Screen.height / 2f);
        base.Raycast(eventData, resultAppendList);
    }
}