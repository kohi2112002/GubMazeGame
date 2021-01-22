using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Leguar.TotalJSON;
using System.Collections;

public class SaveGame : MonoBehaviour
{
    private string isOpen;
    private bool _isOpen;
    private Vector3 initial_pos;
    private const int startSCORE = 0;
    private Vector3[] _pre_pos;

    public GameObject MazeRenderer = null;
    public GameObject Player_1 = null;
    public GameObject Player_2 = null;
    public GameObject Wall;
    public GameObject Food = null;
    public GameObject GameManager;
    public bool isReady;
    public bool _isClicked = false;
    public bool _isUnLocked;
    public int finalSCORE;

    private void Start()
    {
        // Set onclick for stage
        Transform child_1 = transform.GetChild(1);
        Button btn_stage = child_1.GetComponent<Button>(); 
        btn_stage.onClick.AddListener(_stage_on_Clicked);

        //Get data code
        Transform show_txt = transform.GetChild(5);
        Text txt = show_txt.GetComponent<Text>();
        isOpen = "Stage" + txt.text;

        //Set value for initial pos
        initial_pos = new Vector3(-7,0.1f,9);

        //Find prepared position to spawn food
        _pre_pos = prepared_food_pos(15,20);
        
        //Set the number of star
        SetStar();
        Set_Scale_Stage();

        //Test
        CheckUnLocked();

        Debug.Log(GetAndroidExternalStoragePath());

    }

    #region Stage_Info_Controller
    private void _stage_on_Clicked()
    {
        CheckifStage_Opened();  
        _isClicked = true;
        if(!_isOpen)
        {
            IniGame_notisopen();
        }
        else
        {
            //Get value and load map
            GetMap();
        }
        StartCoroutine(IniFood(Food, 50));    
        Set_Score_Ex(); 
        StartCoroutine(Timer(1,3,60));        
        StartCoroutine(DelayForGameManager());

        //Show stage number
        var Stage_Show_Number = GameObject.FindWithTag("stg_num").GetComponent<Text>();        
        if(isOpen == "Stage")
        {
            Stage_Show_Number.text = "Tutorial";
        }
        else
        {
            Stage_Show_Number.text = isOpen.ToString();
        }
    }

    private void CheckifStage_Opened()
    {
        string a = PlayerPrefs.GetString(isOpen);
        if(a == "Open")
        {
            _isOpen = true;                       
        }
        else 
        {
            _isOpen = false;
            PlayerPrefs.SetString(isOpen, "Open");
        }
    }

    private void SetStar()
    {
        int _finalSCORE = PlayerPrefs.GetInt(isOpen + "SCORE", 0); //Get save point
        var a = transform.GetChild(2);
        var b = transform.GetChild(3);
        var c = transform.GetChild(4);

        if(_finalSCORE > 0 && _finalSCORE <=20)
        {
            a.gameObject.SetActive(true);
            b.gameObject.SetActive(false);
            c.gameObject.SetActive(false);
        }
        else if(_finalSCORE > 20 && _finalSCORE <=40)
        {
            a.gameObject.SetActive(true);
            b.gameObject.SetActive(true);
            c.gameObject.SetActive(false);
        }
        else if(_finalSCORE > 40)
        {
            a.gameObject.SetActive(true);
            b.gameObject.SetActive(true);
            c.gameObject.SetActive(true);
        }
        else if(_finalSCORE == 0)
        {
            a.gameObject.SetActive(false);
            b.gameObject.SetActive(false);
            c.gameObject.SetActive(false);
        }
    }

    private void Set_Scale_Stage()
    {
        var _scr_height = Screen.height;
        var base_hei = (_scr_height/1280)*0.2f;
        gameObject.transform.localScale = new Vector3(base_hei, base_hei, 0);
    }

    private void CheckUnLocked()
    {
        if(transform.name == "Stage1")
        {
            if(!UNLOCK())
            {
                SaveUnLock();                
            }
        }

        int _finalSCORE = PlayerPrefs.GetInt(isOpen + "SCORE", 0);
        if(_finalSCORE > 0)
        {           
            SaveUnLock();
            OpenNextMap();
        }
    }

    private void OpenNextMap()
    {
        string name = transform.name;
        char finalchar = name[name.Length - 1];            
        int stagenum = (int)char.GetNumericValue(finalchar);
        int next_stagenum = stagenum+1;
        string next_name = "Stage" + next_stagenum.ToString();    
        GameObject Next_Game = GameObject.Find(next_name);
        if(Next_Game != null)
        {
            Transform img_lock = Next_Game.transform.GetChild(7);
            if(img_lock != null)
            {
                img_lock.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region GameController  
    private void IniPLayer(GameObject pl_1, GameObject pl_2)
    {
        Instantiate(pl_1, initial_pos, Quaternion.identity);
    }
    IEnumerator IniFood(GameObject food, int _foodnumber)
    {
        var a = GameObject.Find("Canvas_MenuScreen").GetComponent<Canvas>();        
        if(_isClicked)
        {
            for(int k = 0; k< _foodnumber;k++)
            {
                //Check Menu open
                if(a.enabled)
                {
                    DestroyAllGate();
                }
                else
                {
                    int i = Random.Range(0,_pre_pos.Length);
                    Instantiate(food, _pre_pos[i], Quaternion.identity);
                    yield return new WaitForSeconds(5);
                }                
            }
        }        
    }

    private void DestroyAllGate()
    {
        GameObject[] gates = GameObject.FindGameObjectsWithTag("food");
        if(gates.Length > 0)
        {
            for(int i = 0; i < gates.Length; i++)
            {
                Destroy(gates[i]);
            }
        }
    }
    private void IniGame_notisopen()
    {
        GameObject Maze = Instantiate(MazeRenderer, MazeRenderer.transform.position, Quaternion.identity);
        IniPLayer(Player_1,Player_2);
    }
    private Vector3[] prepared_food_pos(int row, int column) 
    {
        Vector3[] gr_pos = new Vector3[row*column];
        int j = 0;
        float distance = 1;

        
        for(int i = 1; i <= (row-1)/2; i++)
        {
            for(int k = 1; k <= (column/2); k++)
            {
                Vector3 base_pos_1 = new Vector3(0 - i*distance, 0.1f, 0 + k*distance - 1);
                gr_pos[j] = base_pos_1; j++;
                Vector3 base_pos_2 = new Vector3(0 + i*distance, 0.1f, 0 + k*distance - 1);
                gr_pos[j] = base_pos_2; j++;
                Vector3 base_pos_3 = new Vector3(0 - i*distance, 0.1f, 0 - k*distance);
                gr_pos[j] = base_pos_3; j++;
                Vector3 base_pos_4 = new Vector3(0 + i*distance, 0.1f, 0 - k*distance);
                gr_pos[j] = base_pos_4; j++;
            }
        }
        return gr_pos;
    }
    public void Menu_onClick()
    {
        SaveMap(); //Saving AfterClosing
        GameObject Canvas_MenuScreen = GameObject.Find("Canvas_MenuScreen");
        CheckUnLocked();

        Destroy(GameObject.FindWithTag("GPF"));
        var a = GameObject.Find("GameController").GetComponent<FPath_SPath>();
        if(a != null)
        {
            Debug.Log("Found");
            a.used_number = 3;
        }
        
        if(Canvas_MenuScreen != null)
        {
            Canvas com = Canvas_MenuScreen.GetComponent<Canvas>();
            com.enabled = true;
        }

        GameObject Canvas_InGame = GameObject.Find("Canvas_Ingame");

        if(Canvas_InGame != null)
        {
            Canvas k = Canvas_InGame.GetComponent<Canvas>();
            k.enabled = false;
        }

        var hint = GameObject.Find("btn_Hint").GetComponent<Image>();       
        var num_hint = GameObject.Find("num_hint_left").GetComponent<Text>();        
        hint.enabled = false;
        num_hint.enabled = false;

        Delete_Map_After_Closing();
    }
    IEnumerator Timer(int delay_seconds, int total_minutes, int total_seconds)
    {
        var txt_timer = GameObject.FindWithTag("timer").GetComponent<Text>();
        for(int i = (total_minutes-1);i>=0;i--)
        {
            for(int j = (total_seconds-1); j>=0;j--)
            {           
                txt_timer.text = "0" + i.ToString() + ":" + j.ToString();
                if(j == 0)
                {
                    if(i == 0)
                    {
                        Endgame();
                    }
                }                                
                yield return new WaitForSeconds(delay_seconds); 
            }
        }
    }

    IEnumerator DelayForGameManager()
    {
        var hint = GameObject.Find("btn_Hint").GetComponent<Image>();       
        var num_hint = GameObject.Find("num_hint_left").GetComponent<Text>();
        hint.enabled = false;
        num_hint.enabled = false;
        yield return new WaitForSeconds(30);                 
        hint.enabled = true;
        num_hint.enabled = true;
    }

    private void Set_Score_Ex()
    {
        finalSCORE = startSCORE;
    }

    private void Endgame()
    {
        //Show 3 Buttons
        GameObject cv_3_buttons = GameObject.Find("Canvas_Back");
        Canvas a = cv_3_buttons.GetComponent<Canvas>();
        if(!a.enabled)
        {
            a.enabled = true;
        }
        //Save Score
        SaveScore(finalSCORE);
    }

    public void Retry()
    {        
        SaveMap();
        Delete_Map_After_Closing();
        _stage_on_Clicked();
    }

    public void Next()
    {
        SaveMap();
        Delete_Map_After_Closing();
    }
    public void CreateGame()
    {
        _stage_on_Clicked();
    }
    #endregion

    #region Map Controller
    private string GetAndroidExternalStoragePath()
    {
        return Application.persistentDataPath;

        /*var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", 
            jc.GetStatic<string>("DIRECTORY_DCIM"))
            .Call<string>("getAbsolutePath");
        return path;*/        
    }
    private void SaveMap()
    {
        if(_isClicked)
        {
            //Create filepath
            string filepath = GetAndroidExternalStoragePath() + isOpen + ".json";

            //Create a mapfile 
            JSON Map = new JSON();
            Map.Add("number", isOpen);

            //Adding information to map text file
            GameObject[] wall_existed = GameObject.FindGameObjectsWithTag("wall");
            if(wall_existed.Length > 0)
            {
                Map.Add("length", wall_existed.Length);
                for(int i = 0; i<wall_existed.Length;i++)
                {
                    float x = wall_existed[i].transform.position.x;
                    float y_rot = wall_existed[i].transform.eulerAngles.y;
                    float z = wall_existed[i].transform.position.z;

                    Map.Add(i.ToString(), new float[3]{x,y_rot,z});
                }
            }

            //Save map
            string jsonAsString = Map.CreateString(); 
			StreamWriter writer = new StreamWriter(filepath);
			writer.WriteLine(jsonAsString);
			writer.Close();
        }
    }

    private void GetMap()
    {
        IniPLayer(Player_1,Player_2);
        
        if(_isClicked)
        {   
            //load mapfile form textfile
            JSON loaded_Map = LoadMap();
            
            //Reinstantiate map
            int len = loaded_Map.GetInt("length");
            for(int i = 0; i< len;i++)
            {
                JArray base_array = loaded_Map.GetJArray(i.ToString());
                float x = base_array.GetFloat(0);
                float y_rot = base_array.GetFloat(1); //Get postition data
                float z = base_array.GetFloat(2);
                Vector3 base_pos = new Vector3(x, 0, z);
                Instantiate(Wall, base_pos, Quaternion.Euler(0,y_rot,0));
            }

            GameObject gm = GameObject.Find("GameController");
            if(gm != null)
            {
                var a = gm.GetComponent<FPath_SPath>();
                a.BuildNav();
            }   

            Instantiate(GameManager, new Vector3(0,0,0), Quaternion.identity); //Create map         
        }

    }

    private JSON LoadMap()
    {
        string filepath = null;
        if(_isClicked)
        {
            //Get filepath
            filepath = GetAndroidExternalStoragePath() + isOpen + ".json";
        }

        StreamReader reader = new StreamReader(filepath); 
		string jsonAsString = reader.ReadToEnd();
		reader.Close();
		JSON map = JSON.ParseString(jsonAsString);

        return map;
    }

    private void Delete_Map_After_Closing()
    {
        GameObject[] wall_existed = GameObject.FindGameObjectsWithTag("wall");
        if(wall_existed.Length > 0)
        {
            for(int i = 0; i<wall_existed.Length;i++)
            {
                Destroy(wall_existed[i]);
            }
        }

        Destroy(GameObject.FindWithTag("Main_Player"));
        _isClicked = false;

        GameObject[] _food = GameObject.FindGameObjectsWithTag("food");
        if(_food.Length > 0)
        {
            for(int i = 0; i< _food.Length;i++)
            {
                Destroy(_food[i]);
            }
        }
    }
    #endregion

    #region Collision
    public void Score_Inscrease(int Score_point)
    {
        finalSCORE +=Score_point;
    }

    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt(isOpen + "SCORE", score);
    }
    #endregion

    #region Star_Unlock_System
    public bool UNLOCK()
    {
        string a = PlayerPrefs.GetString(isOpen + "_isUnLock", "false");
        if(a == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveUnLock()
    {
        PlayerPrefs.SetString(isOpen + "_isUnLock", "true");
    }
    #endregion

}
