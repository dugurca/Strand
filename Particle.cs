using UnityEngine;
using System.Collections;

public class StrandParticle
{
    public float Mass { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 LastPosition { get; set; }

    public StrandParticle()
    {

    }

    public StrandParticle(float m, Vector3 p)
    {
        Mass = m;
        Position = p;
        LastPosition = p;
    }

    public StrandParticle(float m, Vector3 p, Vector3 lp)
    {
        Mass = m;
        Position = p;
        LastPosition = lp;
    }
}
