using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ＣＰＵ演算に関連する関数だけもつobject
public class DivNBy1 : MonoBehaviour
{
    int[] nmr = { 5, 0, 8, 6, 2, 2, 0 };//Bellard式の分子が2の何乗か
    int[] nmrsign = { 1, 1, 0, 1, 1, 1, 0 };//Bellard式の分子の符号
    int[] den0 = { 4, 4, 10, 10, 10, 10, 10 };//Bellard式の分母のkの係数
    int[] den1 = { 1, 3, 1, 3, 5, 7, 9 };//Bellard式の分母の端数項


    void umul(ulong u, ulong v, ref ulong h, ref ulong l)
    {
        ulong u1 = (u & 0x00000000ffffffff);
        ulong v1 = (v & 0x00000000ffffffff);
        ulong t = (u1 * v1);
        ulong w3 = (t & 0x00000000ffffffff);
        ulong k = (t >> 32);

        u >>= 32;
        t = (u * v1) + k;
        k = (t & 0x00000000ffffffff);
        ulong w1 = (t >> 32);

        v >>= 32;
        t = (u1 * v) + k;
        k = (t >> 32);

        h = (u * v) + w1 + k;
        l = (t << 32) + w3;
    }

    ulong umullo(ulong a, ulong b)
    {
        return a * b;
    }


    ulong umulhi(ulong u, ulong v)
    {
        ulong u1 = (u & 0x00000000ffffffff);
        ulong v1 = (v & 0x00000000ffffffff);
        ulong t = (u1 * v1);
        ulong w3 = (t & 0x00000000ffffffff);
        ulong k = (t >> 32);

        u >>= 32;
        t = (u * v1) + k;
        k = (t & 0x00000000ffffffff);
        ulong w1 = (t >> 32);

        v >>= 32;
        t = (u1 * v) + k;
        k = (t >> 32);

        ulong h = (u * v) + w1 + k;
        return h;
    }


    ulong CreateTable(ulong idx)
    {
        return ((1UL << 19) - 3UL * (1UL << 8)) / (idx + 256UL);
    }

    /* Algorithm 2 from Mﾃｶller and Granlund
       "Improved division by invariant integers". */
    ulong reciprocal_word(ulong d)
    {
        ulong d0, d9, d40, d63, v0, v1, v2, ehat, v3, v4, hi, lo;
        hi = 0; lo = 0;
        /*
        const uint table[] = {
         Generated with:
           for (int i = (1 << 8); i < (1 << 9); i++)
                   printf("0x%03x,\n", ((1 << 19) - 3 * (1 << 8)) / i); 

        0x7fd, 0x7f5, 0x7ed, 0x7e5, 0x7dd, 0x7d5, 0x7ce, 0x7c6, 0x7bf, 0x7b7,
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
        0x40a, 0x408, 0x406, 0x404, 0x402, 0x400
        };*/

        d0 = d & 1UL;
        d9 = d >> 55;
        d40 = (d >> 24) + 1UL;
        d63 = (d >> 1) + d0;
        v0 = CreateTable(d9 - 256UL);
        //v0 = table[d9 - (1 << 8)];
        v1 = (v0 << 11) - (umullo(umullo(v0, v0), d40) >> 40) - 1UL;
        v2 = (v1 << 13) + (umullo(v1, (1UL << 60) - umullo(v1, d40)) >> 47);
        ehat = (v2 >> 1) * d0 - umullo(v2, d63);
        v3 = (v2 << 31) + (umulhi(v2, ehat) >> 1);
        umul(v3, d, ref hi, ref lo);
        if (lo + d < lo) d++;
        v4 = v3 - (hi + d);
        return v4;
    }

    /* Algorithm 4 from Mﾃｶller and Granlund
       "Improved division by invariant integers".
       Divide u1:u0 by d, returning the quotient and storing the remainder in r.
       v is the approximate reciprocal of d, as computed by reciprocal_word. */
    ulong div2by1(ulong u1, ulong u0, ulong d, ref ulong r, ulong v)
    {
        ulong q0 = 0, q1 = 0;
        umul(v, u1, ref q1, ref q0);
        q0 = q0 + u0;
        if (q0 < u0) q1++;
        q1 = q1 + u1;
        q1++;
        r = u0 - umullo(q1, d);
        q1 = (r > q0) ? q1 - 1 : q1;
        r = (r > q0) ? r + d : r;
        if (r >= d)
        {
            q1++;
            r -= d;
        }
        return q1;
    }
    /* Count leading zeros. */
    
    int clz(ulong x)
    {
            int n = 0;
            while ((x << n) <= 9223372036854775807UL) n++;
            return n;
    }
    

    /* Right-shift that also handles the 64 case. */
    ulong shr(ulong x, int n)
    {
        return n < 64 ? (x >> n) : 0UL;
    }

    /* Divide n-place integer u by d, yielding n-place quotient q. */
    public void divnby1(int n, ulong[] u, ulong d, ulong[] q)
    {
        ulong v, k, ui;
        int l, i;
        /* Normalize d, storing the shift amount in l. */
        l = clz(d);
        d <<= l;
        /* Compute the reciprocal. */
        v = reciprocal_word(d);
        /* Perform the division. */
        k = shr(u[n - 1], 64 - l);
            for (i = n - 1; i >= 1; i--) {
                ui = (u[i] << l) | shr(u[i - 1], 64 - l);
                q[i] = div2by1(k, ui, d, ref k, v);
            }
        q[0] = div2by1(k, u[0] << l, d, ref k, v);
    }



    /* Compute x * y mod n, where n << s is normalized and
       v is the approximate reciprocal of n << s. */
    ulong mulmodn(ulong x, ulong y, ulong n, int s, ulong v)
    {
        //ulong hi, lo, r;
        ulong hi=0, lo = 0;
        umul(x, y, ref hi, ref lo);
        //uint4 out4 = u2mymul128(x, y);
        //lo = out4.xy;
        //hi = out4.zw;
        ulong r=0;
        ulong q1 = div2by1( (hi<<s) | (lo>>(64 - s)), lo<<s, n<<s,ref r, v);
        
        return r>>s;
    }

    /* Compute x^p mod n by means of left-to-right binary exponentiation. */
    public ulong powmodn(ulong x, ulong p, ulong n)
    {
        ulong res, v;
        int i, l, s;
        s = clz(n);

        v = reciprocal_word(n<<s);

        res = x;
        l = 63 - clz(p);

        int ttt = 0;

        for (i = l - 1; i >= 0; i--)
        {
            res = mulmodn(res, res, n, s, v);
            ulong tmp = p>>i;
            if (tmp % 2 == 1)
            {
                res = mulmodn(res, x, n, s, v);
            }
            ttt++;
            if (ttt == 64) break;
        }
        return res;


    }





    public void ul3add(ref ulong b0, ref ulong b1, ref ulong b2,ulong a0,ulong a1,ulong a2) 
    {
        b0 += a0;
        if (b0 < a0)
        {
            b1++;
            if (b1 == 0) b2++;
        }
        b1 += a1;
        if (b1 < a1)
        {
            b2++;
        }
        b2 += a2;
    }

    public void ul3shift(ref ulong tmpans0, ref ulong tmpans1, ref ulong tmpans2, int shiftnum) {
        //分子をかける＝シフト
        if (shiftnum != 0)
        {
            tmpans2 <<= shiftnum;
            tmpans2 += tmpans1 >> (64 - shiftnum);
            tmpans1 <<= shiftnum;
            tmpans1 += tmpans0 >> (64 - shiftnum);
            tmpans0 <<= shiftnum;
        }
    }









    //lastというが実は最初のk=0～255のときも計算している
    public (ulong, ulong, ulong) Addlast(ulong d,int step)
    {
        ulong tmpans0 = 0;
        ulong tmpans1 = 0;
        ulong tmpans2 = 0;
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
            divnby1(4, u, dnm, q);

            if ((k % 2 + (ulong)nmrsign[step]) % 2 == 1)
            {
                q[0] = ~q[0];//bit反転は999999999-qしてるのと同じ
                q[1] = ~q[1];
                q[2] = ~q[2];
                ul3add(ref tmpans0, ref tmpans1, ref tmpans2, 1, 0, 0);
            }
            ul3add(ref tmpans0, ref tmpans1, ref tmpans2, q[0], q[1], q[2]);
        }


        //k=0のとき～k=255のとき
        ulong nmrx;
        ulong nmrp;
        for (ulong iii = 0; iii < 256; iii++)
        {
            if (d <= iii) break;
            nmrx = 1;
            nmrp = 1024;
            dnm = (ulong)den1[step] + (ulong)den0[step] * iii;

            for (ulong dd = d - iii; dd != 0; dd /= 2)
            { //べき剰余
                if (dd % 2 == 1)
                {
                    nmrx = nmrx * nmrp % dnm;
                }
                nmrp = nmrp * nmrp % dnm;
            }

            u[0] = 0; u[1] = 0; u[2] = 0; u[3] = nmrx;
            divnby1(4, u, dnm, q);

            if (((ulong)nmrsign[step] + iii) % 2 == 1)
            {
                q[0] = ~q[0];//bit反転は999999999-qしてるのとほぼ同じ
                q[1] = ~q[1];
                q[2] = ~q[2];
                ul3add(ref tmpans0, ref tmpans1, ref tmpans2, 1, 0, 0);
            }
            ul3add(ref tmpans0, ref tmpans1, ref tmpans2, q[0], q[1], q[2]);
        }

        return (tmpans0, tmpans1, tmpans2);
    }








}


