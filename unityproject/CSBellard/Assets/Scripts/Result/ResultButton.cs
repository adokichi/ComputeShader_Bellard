using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButton : MonoBehaviour
{

    [SerializeField]
    GameObject gPU_Benchmark_Scene;
    [SerializeField]
    GameObject gPU_Benchmark_Result;

    [SerializeField]
    GameObject result_progress_obj;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        gPU_Benchmark_Scene.SetActive(true);
        gPU_Benchmark_Result.SetActive(false);
        result_progress_obj.GetComponent<Result_Progress_Bar>().TexCntInit();

    }
}
