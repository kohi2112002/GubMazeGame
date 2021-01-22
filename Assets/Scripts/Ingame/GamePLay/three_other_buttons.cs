using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class three_other_buttons : MonoBehaviour
{
    public Button btn_Menu;
    public Button btn_Next;
    public Button btn_Retry;
    public Canvas cv_3_buttons;
    void Start()
    {
        btn_Menu.onClick.AddListener(btn_Menu_OnClick);
        btn_Next.onClick.AddListener(btn_Next_OnClick);
        btn_Retry.onClick.AddListener(btn_Retry_OnClick);
    }

    private void btn_Menu_OnClick()
    {
        GameObject[] stage = GameObject.FindGameObjectsWithTag("Stage");
        if(stage != null)
        {
            for(int i = 0;i<stage.Length;i++)
            {
                var a = stage[i].GetComponent<SaveGame>();
                if(a!= null)
                {
                    if(a._isClicked)
                    {
                        a.Menu_onClick();                        
                    }
                }
            }
        }

        cv_3_buttons.GetComponent<Canvas>();
        if(cv_3_buttons.enabled)
        {
            cv_3_buttons.enabled = false;
        }

        var hint = GameObject.Find("btn_Hint").GetComponent<Image>();               
        hint.enabled = true;
    }
    private void btn_Next_OnClick()
    {
        GameObject[] stage = GameObject.FindGameObjectsWithTag("Stage");
        if(stage != null)
        {
            for(int i = 0;i<stage.Length;i++)
            {
                var a = stage[i].GetComponent<SaveGame>();
                if(a!= null)
                {
                    if(a._isClicked)
                    {              
                        a.Next();          
                        var b = stage[i+1].GetComponent<SaveGame>();
                        b.CreateGame();
                    }
                }
            }
        }

        cv_3_buttons.GetComponent<Canvas>();
        if(cv_3_buttons.enabled)
        {
            cv_3_buttons.enabled = false;
        }

        var hint = GameObject.Find("btn_Hint").GetComponent<Image>();               
        hint.enabled = true;
    }
    private void btn_Retry_OnClick()
    {
        GameObject[] stage = GameObject.FindGameObjectsWithTag("Stage");
        if(stage != null)
        {
            for(int i = 0;i<stage.Length;i++)
            {
                var a = stage[i].GetComponent<SaveGame>();
                if(a!= null)
                {
                    if(a._isClicked)
                    {
                        a.Retry();                 
                    }
                }
            }
        }

        cv_3_buttons.GetComponent<Canvas>();
        if(cv_3_buttons.enabled)
        {
            cv_3_buttons.enabled = false;
        }

        var hint = GameObject.Find("btn_Hint").GetComponent<Image>();               
        hint.enabled = true;
    }
    
}
