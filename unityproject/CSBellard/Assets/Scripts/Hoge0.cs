﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoge0 : MonoBehaviour
{
    public DivNBy1 divNBy1;

    public ComputeShader shader;

    int gridn = 2048;
    int blockn = 256;

    ulong debug0 = 0;
    ulong cpuoffset = 39999999;

    ComputeBuffer A;
    ComputeBuffer B;
    ComputeBuffer bigSum;
    ComputeBuffer lastreslut;
    ComputeBuffer constTable;
    ComputeBuffer dbg;
    int k2,kernelSum,kernelZeroset;
    ulong d, offset, k_max, k_max_end;
    int[] inputint2;

    public int step;//式0-6のうちどれを計算しているか
    int[] nmr = { 5, 0, 8, 6, 2, 2, 0 };//Bellard式の分子が2の何乗か
    int[] nmrsign = { 1, 1, 0, 1, 1, 1, 0 };//Bellard式の分子の符号
    int[] den0 = { 4, 4, 10, 10, 10, 10, 10 };//Bellard式の分母のkの係数
    int[] den1 = { 1, 3, 1, 3, 5, 7, 9 };//Bellard式の分母の端数項
    ulong ans0 = 0, ans1 = 0, ans2 = 0;
    float starttime;



    void ulongtouint2(ulong a,int[] b) {
        b[0] = (int)(a % (ulong)4294967296);
        b[1] = (int)(a / (ulong)4294967296);
    }


    void Start()
    {
        divNBy1 = GetComponent<DivNBy1>();

        A = new ComputeBuffer(gridn * blockn * 3, sizeof(ulong));
        B = new ComputeBuffer(gridn * blockn * 3, sizeof(ulong));
        bigSum = new ComputeBuffer(gridn * blockn * 3, sizeof(ulong));
        constTable = new ComputeBuffer(256, sizeof(uint));
        dbg = new ComputeBuffer(2, sizeof(ulong));

        k2 = shader.FindKernel("Sglobal64mtg_192_Refresh");
        kernelSum = shader.FindKernel("SumGlobal");
        kernelZeroset = shader.FindKernel("Zeroset");

        //データセット
        uint[] host_consttable = { 0x7fd, 0x7f5, 0x7ed, 0x7e5, 0x7dd, 0x7d5, 0x7ce, 0x7c6, 0x7bf, 0x7b7,
    0x7b0, 0x7a8, 0x7a1, 0x79a, 0x792, 0x78b, 0x784, 0x77d, 0x776, 0x76f,
    0x768, 0x761, 0x75b, 0x754, 0x74d, 0x747, 0x740, 0x739, 0x733, 0x72c,
    0x726, 0x720, 0x719, 0x713, 0x70d, 0x707, 0x700, 0x6fa, 0x6f4, 0x6ee,
    0x6e8, 0x6e2, 0x6dc, 0x6d6, 0x6d1, 0x6cb, 0x6c5, 0x6bf, 0x6ba, 0x6b4,
    0x6ae, 0x6a9, 0x6a3, 0x69e, 0x698, 0x693, 0x68d, 0x688, 0x683, 0x67d,
    0x678, 0x673, 0x66e, 0x669, 0x664, 0x65e, 0x659, 0x654, 0x64f, 0x64a,
    0x645, 0x640, 0x63c, 0x637, 0x632, 0x62d, 0x628, 0x624, 0x61f, 0x61a,
    0x616, 0x611, 0x60c, 0x608, 0x603, 0x5ff, 0x5fa, 0x5f6, 0x5f1, 0x5ed,
    0x5e9, 0x5e4, 0x5e0, 0x5dc, 0x5d7, 0x5d3, 0x5cf, 0x5cb, 0x5c6, 0x5c2,
    0x5be, 0x5ba, 0x5b6, 0x5b2, 0x5ae, 0x5aa, 0x5a6, 0x5a2, 0x59e, 0x59a,
    0x596, 0x592, 0x58e, 0x58a, 0x586, 0x583, 0x57f, 0x57b, 0x577, 0x574,
    0x570, 0x56c, 0x568, 0x565, 0x561, 0x55e, 0x55a, 0x556, 0x553, 0x54f,
    0x54c, 0x548, 0x545, 0x541, 0x53e, 0x53a, 0x537, 0x534, 0x530, 0x52d,
    0x52a, 0x526, 0x523, 0x520, 0x51c, 0x519, 0x516, 0x513, 0x50f, 0x50c,
    0x509, 0x506, 0x503, 0x500, 0x4fc, 0x4f9, 0x4f6, 0x4f3, 0x4f0, 0x4ed,
    0x4ea, 0x4e7, 0x4e4, 0x4e1, 0x4de, 0x4db, 0x4d8, 0x4d5, 0x4d2, 0x4cf,
    0x4cc, 0x4ca, 0x4c7, 0x4c4, 0x4c1, 0x4be, 0x4bb, 0x4b9, 0x4b6, 0x4b3,
    0x4b0, 0x4ad, 0x4ab, 0x4a8, 0x4a5, 0x4a3, 0x4a0, 0x49d, 0x49b, 0x498,
    0x495, 0x493, 0x490, 0x48d, 0x48b, 0x488, 0x486, 0x483, 0x481, 0x47e,
    0x47c, 0x479, 0x477, 0x474, 0x472, 0x46f, 0x46d, 0x46a, 0x468, 0x465,
    0x463, 0x461, 0x45e, 0x45c, 0x459, 0x457, 0x455, 0x452, 0x450, 0x44e,
    0x44b, 0x449, 0x447, 0x444, 0x442, 0x440, 0x43e, 0x43b, 0x439, 0x437,
    0x435, 0x432, 0x430, 0x42e, 0x42c, 0x42a, 0x428, 0x425, 0x423, 0x421,
    0x41f, 0x41d, 0x41b, 0x419, 0x417, 0x414, 0x412, 0x410, 0x40e, 0x40c,
    0x40a, 0x408, 0x406, 0x404, 0x402, 0x400};
        constTable.SetData(host_consttable);

        //引数をセット
        shader.SetBuffer(k2, "bigSum", bigSum);
        shader.SetBuffer(k2, "ConstTable", constTable);
        shader.SetBuffer(k2, "dbg", dbg);
        shader.SetBuffer(kernelZeroset, "bigSum", bigSum);
        shader.SetInt("gsize", gridn * blockn);

        //d = 39999999;
        d = cpuoffset;

        step = 0;
        parameset();
        ans0 = 0;
        ans1 = 0;
        ans2 = 0;
        Debug.Log("init end");
        step = 90;
    }










    void parameset()
    {
        offset = cpuoffset * debug0 + 256;//かならず256からはじめるよう
        if (d < offset) offset = d;
        k_max = offset;
        k_max_end = d;

        inputint2 = new int[2];
        ulongtouint2(d, inputint2);

        shader.SetInts("d", inputint2);
        shader.SetInt("numesign", nmrsign[step]);
        shader.SetInt("den0", den0[step]);
        shader.SetInt("den1", den1[step]);
        shader.Dispatch(kernelZeroset, gridn, 1, 1);
        starttime = Time.time;
    }

    public void ButtonPush(ulong inul)
    {
        cpuoffset = inul;
        d = cpuoffset;
        step = 0;
        parameset();
    }


    void DebugAns012(ulong a0,ulong a1,ulong a2) 
    {
        string s = a2.ToString("x16") + a1.ToString("x16") + a0.ToString("x16");
        Debug.Log(s);
    }

    void Calc()
    {
        offset = k_max;
        k_max = k_max + (ulong)gridn * (ulong)blockn * 2;
        if (k_max > k_max_end) k_max = k_max_end;

        ulongtouint2(offset, inputint2);
        shader.SetInts("offset", inputint2);
        ulongtouint2(offset * (ulong)den0[step], inputint2);
        shader.SetInts("offset_mul_den0", inputint2);
        ulongtouint2(k_max, inputint2);
        shader.SetInts("k_max", inputint2);

        // GPUで計算
        shader.Dispatch(k2, gridn, 1, 1);
    }









    // Update is called once per frame
    void Update()
    {
        if (step == 7) {
            
            Debug.Log("anser");
            DebugAns012(ans0, ans1, ans2);
            
            step++;
            //解放
            
        }

        if (step < 7)
        {
            Calc();
            //そのstepで最後まで計算したら
            if (k_max == k_max_end)
            {
                var (ul0,ul1,ul2)=ReducAB();//GPU→CPUで結果を1つにまとめる
                Addlast(ul0, ul1, ul2);//端数足してビットシフトで整ええる
                step++;
                if (step < 7)
                {
                    parameset();
                }
            }

        }

    }













    //これでlastreslutに結果がまとめられる
    (ulong,ulong ,ulong) ReducAB() {
        /*
        shader.SetBuffer(kernelSum, "A", bigSum);
        shader.SetBuffer(kernelSum, "B", B);
        lastreslut = B;
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
        */

        //igridn*64個のブロックに結果がある。1ブロック=ulong*3
        //ulong[] ulres = new ulong[igridn * 64 * 3];

        ulong[] ulres = new ulong[gridn * blockn * 3];
        bigSum.GetData(ulres);//, 0, 0, igridn * 64 * 3

        ulong tmpans0 = 0, tmpans1 = 0, tmpans2 = 0;
        for (int i = 0; i < gridn * 256; i++)
        {
            divNBy1.ul3add(ref tmpans0, ref tmpans1, ref tmpans2, ulres[i * 3 + 0], ulres[i * 3 + 1], ulres[i * 3 + 2]);
        }
        return (tmpans0, tmpans1, tmpans2);
    }

    //実は最初のk=0のときも計算している
    void Addlast(ulong tmpans0, ulong tmpans1,ulong tmpans2)
    {
        ulong dnm;
        ulong[] u = { 0, 0, 0, 0 };
        ulong[] q = { 0, 0, 0, 0 };
        //d-k<=0のときのを計算
        for (int i = 0; i < 31; i++) 
        {
            ulong k = d + (ulong)i;
            dnm = k * (ulong)den0[step] + (ulong)den1[step];
            u[0] = 0; u[1] = 0; u[2] = 0; u[3] = 0;
            int bsup = 192 - i * 10;
            if (bsup < 0) break;
            u[bsup / 64] = (ulong)1 << (bsup % 64);
            divNBy1.divnby1(4, u, dnm, q);

            if ((k % 2 + (ulong)nmrsign[step]) % 2 == 1) 
            {
                q[0] = ~q[0];//bit反転は999999999-qしてるのとほぼ同じ
                q[1] = ~q[1];
                q[2] = ~q[2];
                divNBy1.ul3add(ref tmpans0, ref tmpans1, ref tmpans2, 1, 0, 0);
            }
            divNBy1.ul3add(ref tmpans0, ref tmpans1, ref tmpans2, q[0],q[1],q[2]);
        }


        //k=0のとき～k=255のとき
        ulong nmrx;
        ulong nmrp;
        for (ulong iii = 0; iii < cpuoffset * debug0+256; iii++)
        {
            if (d <= iii) break;
            nmrx = 1;
            nmrp = 1024;
            dnm = (ulong)den1[step]+ (ulong)den0[step]*iii;
            
            for (ulong dd = d-iii; dd != 0; dd /= 2)
            { //べき剰余
                if (dd % 2 == 1)
                {
                    nmrx = nmrx * nmrp % dnm;
                }
                nmrp = nmrp * nmrp % dnm;
            }

            u[0] = 0; u[1] = 0; u[2] = 0; u[3] = nmrx;
            divNBy1.divnby1(4, u, dnm, q);

            if (((ulong)nmrsign[step]+iii) % 2 == 1)
            {
                q[0] = ~q[0];//bit反転は999999999-qしてるのとほぼ同じ
                q[1] = ~q[1];
                q[2] = ~q[2];
                divNBy1.ul3add(ref tmpans0, ref tmpans1, ref tmpans2, 1, 0, 0);
            }
            divNBy1.ul3add(ref tmpans0, ref tmpans1, ref tmpans2, q[0], q[1], q[2]);
        }


        //分子をかける＝シフト
        divNBy1.ul3shift(ref tmpans0, ref tmpans1, ref tmpans2, nmr[step]);
        divNBy1.ul3add(ref ans0, ref ans1, ref ans2, tmpans0, tmpans1, tmpans2);

        
        Debug.Log("step"+step);
        Debug.Log("time=" + (Time.time - starttime));

        DebugAns012(tmpans0, tmpans1, tmpans2);
    }



    private void OnDestroy()
    {
        A.Release();
        B.Release();
        bigSum.Release();
        constTable.Release();
    }
}
