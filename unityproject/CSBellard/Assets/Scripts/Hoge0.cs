using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hoge0 : MonoBehaviour
{
    public ComputeShader shader;
    float[] host_A;
    float[] host_B;
    float[] host_C;

    int gridn = 4096 / 2;
    int blockn = 512;

    ComputeBuffer A;
    ComputeBuffer B;
    ComputeBuffer C;
    ComputeBuffer bigSum;
    int k, k2,cnt;
    ulong d, offset, k_max, k_max_end;
    int[] inputint2;
    int flag;

    void ulongtouint2(ulong a,int[] b) {
        b[0] = (int)(a % (ulong)4294967296);
        b[1] = (int)(a / (ulong)4294967296);
    }

    void Start()
    {
        Debug.Log("init 0");
        host_A = new float[] { 1f, 1f, 1f, 1f };
        host_B = new float[] { 1f, 1f, 1f, 1f };
        host_C = new float[] { 0f, 0f, 0f, 0f };

        A = new ComputeBuffer(host_A.Length, sizeof(float));
        B = new ComputeBuffer(host_B.Length, sizeof(float));
        C = new ComputeBuffer(host_C.Length, sizeof(float));
        bigSum = new ComputeBuffer(gridn * blockn * 3, sizeof(ulong));
        
        k = shader.FindKernel("CSMain");
        k2 = shader.FindKernel("Sglobal64mtg_192_Refresh");
        
        // host to device
        A.SetData(host_A);
        B.SetData(host_B);
        
        //引数をセット
        shader.SetBuffer(k, "A", A);
        shader.SetBuffer(k, "B", B);
        shader.SetBuffer(k, "C", C);
        shader.SetBuffer(k2, "bigSum", bigSum);
        //shader.SetBuffer(k2, "dbg0", dbg0);

        d = 399999999;
        offset =    0;
        k_max = offset;
        k_max_end = d + 1;
        //ulong k_max_end = 137576392425472;

        inputint2 =new int[2];
        ulongtouint2(d, inputint2);
        
        shader.SetInts("d", inputint2);
        shader.SetInt("numesign", 1);
        shader.SetInt("den0", 4);
        shader.SetInt("den1", 1);

        cnt = 0;
        flag = 1;
        Debug.Log("init 1");
    }


    // Update is called once per frame
    void Update()
    {
        if (flag >= 1) 
        {
            flag++;
            offset = k_max;
            k_max = k_max + (ulong)gridn * (ulong)blockn * 8;
            if (k_max > k_max_end) k_max = k_max_end;

            ulongtouint2(offset, inputint2);
            shader.SetInts("offset", inputint2);
            ulongtouint2(k_max, inputint2);
            shader.SetInts("k_max", inputint2);

            // GPUで計算
            shader.Dispatch(k2, gridn, 1, 1);
            //shader.Dispatch(k, 1, 1, 1);
            //C.GetData(host_C);
            if (k_max == k_max_end)
            {
                Debug.Log("gpu calc end!");
                flag = 0;
                endf();
            }
            if (flag == 2)
            {
                C.GetData(host_C);
                Debug.Log("first kernel");
            }
        }

        if (cnt%256==100)Debug.Log(cnt);
        cnt++;
    }

    void endf() {

        // device to host
        C.GetData(host_C);
        //ulong[] host_dbg0=new ulong[1];
        //dbg0.GetData(host_dbg0);

        ulong[] ulres = new ulong[gridn * blockn * 3];
        bigSum.GetData(ulres);
        ulong ans0 = 0, ans1 = 0, ans2 = 0;
        for (int i = 0; i < gridn * blockn; i++)
        {
            ans0 += ulres[i * 3 + 0];
            if (ans0 < ulres[i * 3 + 0])
            {
                ans1++;
                if (ans1 == 0) ans2++;
            }
            ans1 += ulres[i * 3 + 1];
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


}