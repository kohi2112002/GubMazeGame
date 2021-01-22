using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_OnClicked : MonoBehaviour
{
    private void Start()
    {
        //Set Layer for two canvas
        GameObject Canvas_InGame = GameObject.Find("Canvas_Ingame");
        if(Canvas_InGame != null)
        {
            Canvas k = Canvas_InGame.GetComponent<Canvas>();
            k.enabled = false;
        }
        GameObject Canvas_MenuScreen = GameObject.Find("Canvas_MenuScreen");
        if(Canvas_MenuScreen != null)
        {
            Canvas com = Canvas_MenuScreen.GetComponent<Canvas>();
            com.enabled = true;
        }
        //----------------------------------------------------------------

        GameObject Controller = GameObject.Find("Controller");
        if(Controller != null)
        {
            MenuRenderer _menurenderer = Controller.GetComponent<MenuRenderer>();
            if(_menurenderer != null)
            {
                if(_menurenderer._isMenuGenerated)
                {
                    GameObject[] stage_created = GameObject.FindGameObjectsWithTag("Stage");
                    for(int i =0; i< stage_created.Length;i++)
                    {
                        Transform BG = stage_created[i].transform.GetChild(1);
                        Button btn = BG.GetComponent<Button>();
                        btn.onClick.AddListener(Stage_On_CLicked);
                    }
                }
            }
        }        
    }

    private void Stage_On_CLicked()
    {        
        GameObject Canvas_MenuScreen = GameObject.Find("Canvas_MenuScreen");
        if(Canvas_MenuScreen != null)
        {
            Canvas com = Canvas_MenuScreen.GetComponent<Canvas>();
            com.enabled = false;
        }
        GameObject Canvas_InGame = GameObject.Find("Canvas_Ingame");
        if(Canvas_InGame != null)
        {
            Canvas k = Canvas_InGame.GetComponent<Canvas>();
            k.enabled = true;
        }
    }
}
