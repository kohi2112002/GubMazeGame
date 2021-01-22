using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Suitable_Size_Content : MonoBehaviour
{
    public float strectch_number;
    void Start()
    {
        var a = gameObject.GetComponent<RectTransform>();
        if(a != null)
        {
            //a.sizeDelta = new Vector2(a.sizeDelta.x, a.sizeDelta.y*strectch_number);
            //a.anchoredPosition = new Vector2(a.anchoredPosition.x, a.anchoredPosition.y + 100*strectch_number);
        }
    }
}
