using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class FPath_SPath : MonoBehaviour
{
    public Button btnFind;
    public Button btnGO;
    int[] a;
    public int used_number;
    public Text txt_show_used_number;
    private Vector3 target_pos;
    private int cur_choosed;
    public NavMeshSurface surface; 

    private void Start()
    {
        btnFind.onClick.AddListener(btnFind_onClick);
        btnGO.onClick.AddListener(btnGO_onClick);

        used_number = 3;        
    }

    private void btnFind_onClick()
    {        
        cur_choosed = 0;
        if(used_number > 0)       
        {
            used_number --;            
            
            //Get Player Pos
            Vector3 player_pos = GameObject.FindWithTag("Main_Player").transform.position;            
            
            #region Get nearest gate, but still not work =((((((
            /*GameObject[] gates = GameObject.FindGameObjectsWithTag("food");
            if(gates.Length > 0)
            {      
                GameObject pathFIND = GameObject.FindWithTag("GPF");
                if(pathFIND != null)
                {            
                    Pathfinding pathGenerate = pathFIND.GetComponent<Pathfinding>();
                    if(pathGenerate != null)
                    {
                        for(int i = 0; i < gates.Length; i++)
                        {                        
                            Vector3 base_pos = gates[i].transform.position;
                            pathGenerate.FindPath(player_pos, target_pos, false);

                            Grid gr = pathFIND.GetComponent<Grid>();
                            if(gr != null)
                            {                        
                                int[] dis_arr = gr.distance.ToArray();                        
                                if(dis_arr.Length == 1)
                                {
                                    cur_choosed = dis_arr[dis_arr.Length - 1];
                                    target_pos = base_pos;
                                    Debug.Log(cur_choosed.ToString() + base_pos.x.ToString() + base_pos.z.ToString());
                                }
                                else if(dis_arr.Length > 1)
                                {
                                    if(cur_choosed >= dis_arr[dis_arr.Length-1])
                                    {
                                        cur_choosed = dis_arr[dis_arr.Length-1];
                                        target_pos = base_pos;
                                        Debug.Log(cur_choosed.ToString() + base_pos.x.ToString() + base_pos.z.ToString());
                                    }                                    
                                }                                
                            }      
                        }                         
                    }                
                }                                                                                  

                Pathfinding _pathGenerate = pathFIND.GetComponent<Pathfinding>();
                if(_pathGenerate != null)
                {
                    _pathGenerate.FindPath(player_pos, target_pos, true);
                }          
            }*/
            #endregion ------------------------            

            //Get random target point
            GameObject[] gates = GameObject.FindGameObjectsWithTag("food");
            if(gates.Length > 0)
            {
                cur_choosed = Random.Range(0, gates.Length);
                target_pos = gates[cur_choosed].transform.position;

                Food_On_Colission foc = gates[cur_choosed].GetComponent<Food_On_Colission>();
                if(foc != null)
                {
                    foc.is_choosed = true;
                }
            }

            //Find Path
            GameObject pathFIND = GameObject.FindWithTag("GPF");
            if(pathFIND != null)
            {            
                Pathfinding pathGenerate = pathFIND.GetComponent<Pathfinding>();
                if(pathGenerate != null)
                {
                    pathGenerate.FindPath(player_pos, target_pos, true);
                }
            }
        }
    }

    private void btnGO_onClick()
    {
        if(target_pos != Vector3.zero && used_number >= 0)
        {
            GameObject _Player = GameObject.FindWithTag("Main_Player");                         
            PLayer_Controller a = _Player.GetComponent<PLayer_Controller>();
            a.GOTO(target_pos);
        } 
    }

    private void Update()
    {
        txt_show_used_number.text = used_number.ToString();
    }

    public void BuildNav()
    {        
        surface.BuildNavMesh();
    }
}
