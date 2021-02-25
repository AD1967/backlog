using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleSlider : MonoBehaviour, IDragHandler
{
    public GameObject Point;
    public void OnDrag(PointerEventData eventData)
    {
        float Distance = Vector3.Distance(eventData.position, transform.localPosition);
        if (Distance > 400)
        {

            //Vector3 NewPos = 
        }
    }

}
