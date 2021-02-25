using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleSliderHandler : MonoBehaviour, IDragHandler
{
    public Vector2 StartP;
    public Image SliderCircle;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newRotation = new Vector3(0, 0, 0);
        if(StartP.x > eventData.position.x) {
            SliderCircle.fillAmount -= 0.005f;
            newRotation = new Vector3(0, 0, 2.33f + transform.parent.rotation.z);
            transform.parent.Rotate(newRotation);
            return;
        }

        if (SliderCircle.fillAmount < 0.5f)
        {
            SliderCircle.fillAmount += 0.005f;
            newRotation = new Vector3(0, 0, -1.33f + transform.parent.rotation.z);
            transform.parent.Rotate(newRotation);
        }
        else if(SliderCircle.fillAmount > 0.5f)
        {
            SliderCircle.fillAmount = 0.5f;
            transform.parent.Rotate(new Vector3(0,0,-180));

        }
        
    }

    void Start()
    {
        StartP = transform.position;
    }

    
}
