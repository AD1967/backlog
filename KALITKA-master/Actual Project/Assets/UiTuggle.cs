using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiTuggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public static bool Status;
    [SerializeField] private bool IsOn;
    [SerializeField] private GameObject TuggleImg;
    [SerializeField] public UnityEvent Event;
    
    void Start() {
        Status = IsOn;
    }

    void Update()
    {
        
        var Pos = TuggleImg.transform.localPosition;
        if (!Status)
        {
            Vector3 newLocation = new Vector3(-39, Pos.y, Pos.z);
            StartCoroutine(SmoothMove(Pos, newLocation, 0.5f));
        }else if (Status)
        {
            Vector3 newLocation = new Vector3(39, Pos.y, Pos.z);
            StartCoroutine(SmoothMove(Pos, newLocation, 0.5f));
        }
    }


    IEnumerator SmoothMove(Vector3 start, Vector3 end, float second)
    {
        float t = 0;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / second;
            TuggleImg.transform.localPosition = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1f, t));
            yield return null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Status)
        {
            Status = false;
        }
        else
        {
            Status = true;
        }

        Event.Invoke();
    }
}
