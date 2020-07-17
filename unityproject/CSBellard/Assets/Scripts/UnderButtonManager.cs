using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    int mode = 0;
    [SerializeField]
    public Button button0;
    [SerializeField]
    public Button button1;
    [SerializeField]
    public Button button2;

    [SerializeField]
    GameObject gPU_Benchmarks;
    [SerializeField]
    GameObject ranking;
    [SerializeField]
    GameObject setting;
    void Start()
    {
        
    }



    public void Button0()
    {
        button0.interactable = false;
        button1.interactable = true;
        button2.interactable = true;
        gPU_Benchmarks.SetActive(true);
        if (mode == 1) 
        {
            ranking.GetComponent<Ranking>().AllDestroy();
            ranking.GetComponent<Ranking>().cnt = -1;
            ranking.SetActive(false);
        }
        if (mode == 2) 
        {
            setting.SetActive(false);
        }
        mode = 0;
    }
    public void Button1()
    {
        button0.interactable = true;
        button1.interactable = false;
        button2.interactable = true;
        ranking.SetActive(true);
        if (mode == 0)
        {
            gPU_Benchmarks.SetActive(false);
        }
        if (mode == 2) 
        {
            setting.SetActive(false);
        }
        mode = 1;
    }
    public void Button2()
    {
        button0.interactable = true;
        button1.interactable = true;
        button2.interactable = false;
        setting.SetActive(true);
        if (mode == 1)
        {
            ranking.GetComponent<Ranking>().AllDestroy();
            ranking.GetComponent<Ranking>().cnt = -1;
            ranking.SetActive(false);
        }

        if (mode == 0)
        {
            gPU_Benchmarks.SetActive(false);
        }

        mode = 2;
    }
}
