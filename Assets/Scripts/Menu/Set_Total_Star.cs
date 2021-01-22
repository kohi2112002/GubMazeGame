using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set_Total_Star : MonoBehaviour
{
    public Text total_Stars;
    private Canvas Menu;
    private int max;

    private void Start()
    {
        Menu = GameObject.Find("Canvas_MenuScreen").GetComponent<Canvas>();                
    }

    private void Update()
    {
        if(Menu.enabled)
        {
            GameObject[] stages = GameObject.FindGameObjectsWithTag("stage");
            if(stages != null)
            {
                for(int i = 0;i<stages.Length;i++)
                {
                    var a = stages[i].GetComponent<SaveGame>();
                    if(a != null)
                    {
                        if(a._isUnLocked == true)
                        {
                            //max = i;
                        }
                    }
                }
            }
        }
    }
}
