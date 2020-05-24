using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hoge0 : MonoBehaviour
{
    public ComputeShader shader;

    void ulongtouint2(ulong a,int[] b) {
        b[0] = (int)(a % (ulong)4294967296);
        b[1] = (int)(a / (ulong)4294967296);
        //Debug.Log(a);
        //Debug.Log(b[0]);
        //Debug.Log(b[1]);
    }

    void Start()
    {

        float[] host_A = { 1f, 1f, 1f, 1f };
        float[] host_B = { 1f, 1f, 1f, 1f };
        float[] host_C = { 0f, 0f, 0f, 0f };

        ComputeBuffer A = new ComputeBuffer(host_A.Length, sizeof(float));
        ComputeBuffer B = new ComputeBuffer(host_B.Length, sizeof(float));
        ComputeBuffer C = new ComputeBuffer(host_C.Length, sizeof(float));

        ComputeBuffer bigSum = new ComputeBuffer(4096 * 256 * 3, sizeof(ulong));
        //ComputeBuffer dbg0 = new ComputeBuffer(1, sizeof(ulong));

        int k = shader.FindKernel("CSMain");
        int k2 = shader.FindKernel("Sglobal64mtg_192_Refresh");
        
        // host to device
        A.SetData(host_A);
        B.SetData(host_B);
        
        //引数をセット
        shader.SetBuffer(k, "A", A);
        shader.SetBuffer(k, "B", B);
        shader.SetBuffer(k, "C", C);
        shader.SetBuffer(k2, "bigSum", bigSum);
        //shader.SetBuffer(k2, "dbg0", dbg0);


        /*
    uint2 offset;
    uint2 d;
    uint2 k_max;
    uint numesign;
    uint den0;
    uint den1;
    */

        ulong d = 799999999999999;
        ulong offset =    137438953472000;
        ulong k_max = offset;
        ulong k_max_end = 137438993472000;
        //ulong k_max_end = 137576392425472;

        int[] inputint2=new int[2];
        ulongtouint2(d, inputint2);
        
        shader.SetInts("d", inputint2);
        shader.SetInt("numesign", 1);
        shader.SetInt("den0", 4);
        shader.SetInt("den1", 1);

        
        for (;; )
        {
            offset = k_max;
            k_max = k_max + 256 * 4096 * 8;
            if (k_max > k_max_end) k_max = k_max_end;
            ulongtouint2(offset, inputint2);
            shader.SetInts("offset", inputint2);
            ulongtouint2(k_max, inputint2);
            shader.SetInts("k_max", inputint2);
            // GPUで計算
            shader.Dispatch(k2, 4096, 1, 1);
            C.GetData(host_C);
            if (k_max == k_max_end) break;
        }



        // device to host
        C.GetData(host_C);
        //ulong[] host_dbg0=new ulong[1];
        //dbg0.GetData(host_dbg0);

        ulong[] ulres = new ulong[4096 * 256 * 3];
        bigSum.GetData(ulres);
        ulong ans0 = 0, ans1 = 0, ans2 = 0;
        for (int i= 0; i < 4096 * 256;i++) {
            ans0 += ulres[i * 3 + 0];
            if (ans0 < ulres[i * 3 + 0]) {
                ans1++;
                if (ans1 == 0) ans2++;
            }
            ans1+= ulres[i * 3 + 1];
            if (ans1 < ulres[i * 3 + 1])
            {
                ans2++;
            }
            ans2 += ulres[i * 3 + 2];
        }
        //分子をかける＝シフト
        ans2 <<= 5;
        ans2 += ans1 >> 59;
        ans1 <<= 5;
        ans1 += ans0 >> 59;
        ans0 <<= 5;


        Debug.Log(ans0.ToString("x16"));
        Debug.Log(ans1.ToString("x16"));
        Debug.Log(ans2.ToString("x16"));

        /*
        Debug.Log("dnm");
        Debug.Log(host_dbg0[0]);
        Debug.Log(host_dbg0[0].ToString("x4"));
        */

        //解放
        A.Release();
        B.Release();
        C.Release();
        bigSum.Release();
    }


    // Update is called once per frame
    void Update()
    {

    }


}