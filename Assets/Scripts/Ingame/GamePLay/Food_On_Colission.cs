using System.Collections;
using UnityEngine;

public class Food_On_Colission : MonoBehaviour
{
    private int current_stages;
    private GameObject[] stages;
    private int score_point = 1;
    private int delay_time = 3;
    public bool is_choosed;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Main_Player")
        {
            StartCoroutine(Score_Plus(delay_time));

            PLayer_Controller _controller = other.gameObject.transform.GetComponent<PLayer_Controller>();
            if(_controller != null && is_choosed)
            {
                _controller.Agent_TURNOFF();
                is_choosed = false;
            }
        }

        if(other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Get current stage opened
        stages = GameObject.FindGameObjectsWithTag("Stage");
        if(stages.Length > 0)
        {
            for(int i = 0;i<stages.Length;i++)
            {
                SaveGame sv = stages[i].transform.GetComponent<SaveGame>();
                if(sv != null)
                {
                    if(sv._isClicked == true)
                    {
                        current_stages = i;
                    }
                }
            }
        }

        //Set gate_target
        is_choosed = false;
    }

    IEnumerator Score_Plus(int time)
    {
        Destroy(gameObject);
        SaveGame sv = stages[current_stages].transform.GetComponent<SaveGame>();
        if(sv != null)
        {
            sv.Score_Inscrease(score_point);
        }

        yield return new WaitForSeconds(time);
    }
}
