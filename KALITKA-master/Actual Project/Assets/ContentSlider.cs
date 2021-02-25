using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContentSlider : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    public float procentHold = 0.2f;
    public float easing = 0.5f;
    public int Pages;
    public bool Pulling;
    private void Start()
    {
        startPos = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Pulling)
        {
            float moment = eventData.pressPosition.x - eventData.position.x;
            transform.position = startPos - new Vector3(moment, 0f, 0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float moment = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Math.Abs(moment) >= procentHold)
        {
            Vector3 newLocation = startPos;
            if (moment > 0)
            {
                
                if (Pages != -1)
                {
                    newLocation += new Vector3(-Screen.width - 50f, 0, 0);
                    Pages--;
                }
                

            }else if (moment < 0) 
            {
                if (Pages != 1)
                {
                    newLocation += new Vector3(Screen.width + 50f, 0, 0);
                    Pages++;
                }
               
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            startPos = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, startPos, easing));
        }
        
        
    }
    IEnumerator SmoothMove(Vector3 start, Vector3 end, float second)
    {
        float t = 0;
        while (t<= 1.0f)
        {
            t += Time.deltaTime / second;
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1f, t));
            yield return null;
        }
    }
}
