using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hoge0 : MonoBehaviour
{
    public ComputeShader shader;
    float[] host_A;
    float[] host_B;
    float[] host_C;

    int gridn = 4096;
    int blockn = 256;

    ComputeBuffer A;
    ComputeBuffer B;
    ComputeBuffer bigSum;
    int k2,kernelSum,kernelZeroset,cnt;
    ulong d, offset, k_max, k_max_end;
    int[] inputint2;

    int step;//式0-6のうちどれを計算しているか
    int[] nmr = { 5, 0, 8, 6, 2, 2, 0 };//Bellard式の分子が2の何乗か
    int[] nmrsign = { 1, 1, 0, 1, 1, 1, 0 };//Bellard式の分子の符号
    int[] den0 = { 4, 4, 10, 10, 10, 10, 10 };//Bellard式の分母のkの係数
    int[] den1 = { 1, 3, 1, 3, 5, 7, 9 };//Bellard式の分母の端数項
    ulong ans0 = 0, ans1 = 0, ans2 = 0;


    void ulongtouint2(ulong a,int[] b) {
        b[0] = (int)(a % (ulong)4294967296);
        b[1] = (int)(a / (ulong)4294967296);
    }

    void parameset() {

        d = 399999999;
        //d =      799999999999999;
        offset = 0;
        k_max = offset;
        k_max_end = d + 1;//8589934592

        inputint2 = new int[2];
        ulongtouint2(d, inputint2);

        shader.SetInts("d", inputint2);
        shader.SetInt("numesign", nmrsign[step]);
        shader.SetInt("den0", den0[step]);
        shader.SetInt("den1", den1[step]);
        shader.Dispatch(kernelZeroset, gridn, 1, 1);
    }

    void Start()
    {
        Debug.Log("init 0");

        A = new ComputeBuffer(gridn * blockn * 3 / 4, sizeof(ulong));
        B = new ComputeBuffer(gridn * blockn * 3 / 4, sizeof(ulong));
        bigSum = new ComputeBuffer(gridn * blockn * 3, sizeof(ulong));
        
        k2 = shader.FindKernel("Sglobal64mtg_192_Refresh");
        kernelSum = shader.FindKernel("SumGlobal");
        kernelZeroset = shader.FindKernel("Zeroset");

        //引数をセット
        shader.SetBuffer(k2, "bigSum", bigSum);
        shader.SetBuffer(kernelZeroset, "bigSum", bigSum);

        step = 0;
        parameset();

        cnt = 0;
        Debug.Log("init 1");
    }

    void Calc()
    {
        offset = k_max;
        k_max = k_max + (ulong)gridn * (ulong)blockn * 8;
        if (k_max > k_max_end) k_max = k_max_end;

        ulongtouint2(offset, inputint2);
        shader.SetInts("offset", inputint2);
        ulongtouint2(k_max, inputint2);
        shader.SetInts("k_max", inputint2);

        // GPUで計算
        shader.Dispatch(k2, gridn, 1, 1);
        //C.GetData(host_C);

        if (k_max == k_max_end)
        {
            ReducAB();
            step++;
            if (step < 7)
            {
                parameset();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 7) {
            Debug.Log(ans0.ToString("x16"));
            Debug.Log(ans1.ToString("x16"));
            Debug.Log(ans2.ToString("x16"));
            step++;

            //解放
            A.Release();
            B.Release();
            bigSum.Release();
        }

        if (step < 7)
        {
            Calc();
        }

        if (cnt%128==31) Debug.Log(cnt);
        cnt++;
    }



    void ReducAB() {
        shader.SetBuffer(kernelSum, "A", bigSum);
        shader.SetBuffer(kernelSum, "B", B);
        ComputeBuffer lastreslut = B;
        shader.Dispatch(kernelSum, gridn, 1, 1);
        int igridn = gridn;
        
        for (int i = 0; igridn % 4 == 0; igridn /= 4) 
        {
            if (i % 2 == 0)
            {
                shader.SetBuffer(kernelSum, "A", B);
                shader.SetBuffer(kernelSum, "B", A);
                lastreslut = A;
            }
            else
            {
                shader.SetBuffer(kernelSum, "A", A);
                shader.SetBuffer(kernelSum, "B", B);
                lastreslut = B;
            }
            shader.Dispatch(kernelSum, igridn / 4, 1, 1);
            i++;
        }

        //igridn*64個のブロックに結果がある。1ブロック=ulong*3
        ulong[] ulres = new ulong[igridn * 64 * 3];
        lastreslut.GetData(ulres,0,0, igridn * 64 * 3);

        ulong tmpans0 = 0, tmpans1 = 0, tmpans2 = 0;
        for (int i = 0; i < igridn * 64; i++)
        {
            tmpans0 += ulres[i * 3 + 0];
            if (tmpans0 < ulres[i * 3 + 0])
            {
                tmpans1++;
                if (tmpans1 == 0) tmpans2++;
            }
            tmpans1 += ulres[i * 3 + 1];
            if (tmpans1 < ulres[i * 3 + 1])
            {
                tmpans2++;
            }
            tmpans2 += ulres[i * 3 + 2];
        }

        //分子をかける＝シフト
        if (nmr[step] != 0) {
            tmpans2 <<= nmr[step];
            tmpans2 += tmpans1 >> (64 - nmr[step]);
            tmpans1 <<= nmr[step];
            tmpans1 += tmpans0 >> (64 - nmr[step]);
            tmpans0 <<= nmr[step];
        }

        ans0 += tmpans0;
        if (ans0 < tmpans0)
        {
            ans1++;
            if (ans1 == 0) ans2++;
        }
        ans1 += tmpans1;
        if (ans1 < tmpans1)
        {
            ans2++;
        }
        ans2 += tmpans2;

        
        Debug.Log(tmpans0.ToString("x16"));
        Debug.Log(tmpans1.ToString("x16"));
        Debug.Log(tmpans2.ToString("x16"));
    }
}
