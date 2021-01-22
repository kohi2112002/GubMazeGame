using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuRenderer : MonoBehaviour
{
    public GameObject pre_Stage;
    public GameObject Menu_scroll_Content;
    public GameObject Hor_Connection;
    public GameObject Ver_Connection;
    public GameObject Cam;

    public bool _isMenuGenerated;
    private int scr_width;
    private int scr_height;

    private int[] finalstagenumber = new int[1000];
    private int[][] stageMatrix = new int[250][];
    private float hor_width;

    //Variables that controll the size hor connect
    private Vector3 pos_1;
    private Vector3 pos_2;
    private float width_connect;
    private float height_connect;

    void Start()
    {
        scr_width = Screen.width; hor_width = Screen.width/2;
        scr_height = Screen.height;        

        //Change height of cam
        if((scr_height == 2960 && scr_width == 1440) || (scr_height == 2160 && scr_width == 1080))
        {
            Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y + 3f, Cam.transform.position.z);
        }
        Menu_Generating();
    }

    private void Menu_Generating()
    {
        StageNumberPreparation();
        Stage_Generating(scr_width, scr_height);

        GameObject[] stages_created = GameObject.FindGameObjectsWithTag("Stage");
        if (stages_created.Length == 1000)
        {
            _isMenuGenerated = true;
        }
        else
        {
            GameObject[] stage_created = GameObject.FindGameObjectsWithTag("Stage");
            for (int i = 0; i < stage_created.Length; i++)
            {
                GameObject.Destroy(stage_created[i]);
            }
            Stage_Generating(scr_width, scr_height);
        }
    }
    private void Stage_Generating(int scr_wi, int scr_hei)
    {
        float width_distance = scr_wi / 5;
        float height_distance = scr_wi / 4;           

        for (int i = 1; i <= 250; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                if (i == 1 && j == 1)
                {
                    pos_1 = new Vector3(j * width_distance, i * height_distance, 0);                    
                    GameObject Stage_1 = Instantiate(pre_Stage, pos_1 , Quaternion.identity);                
                    Stage_1.transform.SetParent(Menu_scroll_Content.transform, true);
                    Transform lock_state = Stage_1.transform.GetChild(7); lock_state.gameObject.SetActive(false);
                    Transform stage_num = Stage_1.transform.GetChild(5); stage_num.gameObject.SetActive(false);                    
                    Transform tut_1 = Stage_1.transform.GetChild(6); tut_1.gameObject.SetActive(true);                    
                    Transform Connection = Stage_1.transform.GetChild(0); Connection.gameObject.SetActive(false); 
                    Stage_1.name = "Stage1";                               
                }
                else if (i % 2 == 0)
                {
                    if (j == 1)
                    {
                        StageRenderer(width_distance, height_distance, i, j, true, stageMatrix[i-1][j-1]);       
                    }
                    else
                    {
                        StageRenderer(width_distance, height_distance, i, j, false, stageMatrix[i-1][j-1]);                        
                    }
                }
                else if (i % 2 != 0)
                {
                    if (j == 4)
                    {
                        StageRenderer(width_distance, height_distance, i, j, true, stageMatrix[i-1][j-1]);  
                        pos_2 = new Vector3(width_distance*j, height_distance*i, 0);
                        width_connect = Mathf.Abs(pos_1.x - pos_2.x);

                        HorizontalConnectionRenderer(i * height_distance, Hor_Connection);
                    }
                    else
                    {
                        StageRenderer(width_distance, height_distance, i, j, false, stageMatrix[i-1][j-1]);                        
                    }
                }
            }
        }

        GameObject[] stages = GameObject.FindGameObjectsWithTag("Stage");
        for(int i =0;i<stages.Length;i++)
        {
            Transform show_txt = stages[i].transform.GetChild(5);
            Text txt = show_txt.GetComponent<Text>();
            if(txt.text == 1000.ToString())
            {
                Destroy(stages[i]);
            }
        }
    }

    private void StageRenderer(float wid_dis, float hei_dis, int i_scalor, int j_scalor, bool spe_point, int k)
    {        
        //Set Vertical Connection
        if (spe_point)
        {
            var a = ((i_scalor)*hei_dis);
            height_connect = a/i_scalor;
            var base_pos = new Vector3(j_scalor*wid_dis, a + height_connect/2, 0);
            GameObject ver_cor = Instantiate(Ver_Connection, base_pos, Quaternion.identity);
            ver_cor.gameObject.transform.SetParent(Menu_scroll_Content.transform, true);
            var b = ver_cor.gameObject.transform.GetComponent<RectTransform>();
            if(b!=null)
            {
                b.sizeDelta = new Vector2(b.sizeDelta.x, height_connect);
                var _scr_width = Screen.width;
                var base_width = (_scr_width/800)*0.5f;
                b.transform.localScale = new Vector3(base_width, b.transform.localScale.y,0);
            }
        }

        //Ini Stage
        GameObject Stage = Instantiate(pre_Stage, new Vector3(j_scalor * wid_dis, i_scalor * hei_dis, 0), Quaternion.identity);
        Stage.transform.SetParent(Menu_scroll_Content.transform, true);


        //Set Horizontal Connection
        HorizontalConnectionRenderer(i_scalor * hei_dis, Hor_Connection);

        //Delete Tutorial Text
        Transform tut = Stage.transform.GetChild(6); tut.gameObject.SetActive(false);

        //Set stage number
        Transform stage_num_ex_1 = Stage.transform.GetChild(5);
        Text txt = stage_num_ex_1.GetComponent<Text>();
        txt.text = k.ToString();

        //Set stage name
        Stage.name = "Stage" + txt.text;
    }

    private void HorizontalConnectionRenderer(float ver_pos, GameObject connect)
    {
        GameObject _connect = Instantiate(connect, new Vector3(hor_width, ver_pos+1f, 0), Quaternion.identity);
        _connect.transform.SetParent(Menu_scroll_Content.transform, true);
        _connect.transform.SetSiblingIndex(0);
        Set_Scale_hor_connecnt(_connect);
        var size_change = _connect.transform.GetComponent<RectTransform>();
        if(size_change != null)
        {
            size_change.sizeDelta = new Vector2(width_connect, size_change.sizeDelta.y);
        }
    }
    private void Set_Scale_hor_connecnt(GameObject g)
    {
        var _scr_height = Screen.height;
        var base_hei = (_scr_height/800)*0.5f;
        g.transform.localScale = new Vector3(g.transform.localScale.x, base_hei, 0);
    }

    private void StageNumberPreparation()
    {         
        #region Generate a jagged array with interval from 1 to 999        
        int currentstagesnumber = 1;

        for(int i = 0;i< stageMatrix.Length;i++)
        {
            stageMatrix[i] = new int[4];            
            for(int j = 0;j< stageMatrix[i].Length;j++)
            {
                stageMatrix[i][j] = currentstagesnumber;
                currentstagesnumber++;
            }
        }
        #endregion

        #region Get the jagged whose even rows are inverse 
        for(int i=0;i<stageMatrix.Length;i++)
        {
            if(i%2 != 0)
            {
                Array.Reverse(stageMatrix[i]);
            }
        }
        #endregion

        #region Convert jagged array to one dimensional array        
        int k = 0;
        for(int i = 0 ;i < stageMatrix.Length;i++)
        {
            for(int j = 0;j < stageMatrix[i].Length;j++)
            {
                finalstagenumber[k] = stageMatrix[i][j];
                k++;                            
            }
        }   
        #endregion  
    }
}
