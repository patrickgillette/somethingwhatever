using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColonyNameplate : MonoBehaviour, IPointerClickHandler {

    public Colony MyColony;

    public void OnPointerClick(PointerEventData eventData)
    {
        //MapObjectNamePlate monp = GetComponent<MapObjectNamePlate>();

        GameObject.FindObjectOfType<SelectionController>().SelectedColony = MyColony;
    }
}
