using UnityEngine;
using System.Collections;

public class Spring
{
    public int P1 { get; set; }
    public int P2 { get; set; }
    public int Type { get; set; }
    public float RestLength { get; set; }
    public float Ks { get; set; }
    public float Kd { get; set; }
    public float MaxStretch { get; set; }

    public Spring()
    {

    }

    public Spring(int p1, int p2, float restLength, float ks, float kd)
    {
        P1 = p1;
        P2 = p2;
        RestLength = restLength;
        Ks = ks;
        Kd = kd;
    }
}
