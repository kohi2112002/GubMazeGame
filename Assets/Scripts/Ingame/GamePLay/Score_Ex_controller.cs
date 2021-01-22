using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Ex_controller : MonoBehaviour
{
    private Image img_show;
    void Start()
    {
        img_show = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var stages = GameObject.FindGameObjectsWithTag("Stage");
        if(stages.Length > 0)
        {
            for(int i = 0;i<stages.Length;i++)
            {
                SaveGame sv = stages[i].transform.GetComponent<SaveGame>();
                if(sv != null)
                {
                    if(sv._isClicked == true)
                    {
                        img_show.fillAmount = (sv.finalSCORE)*0.02f;
                    }
                }
            }
        }
    }
}
