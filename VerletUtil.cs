using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerletUtil
{

    public static Vector3 GetVerletVelocity(Vector3 pos, Vector3 lastPos, float dt)
    {
        return (pos - lastPos) / dt;
    }

    public static Vector3 AddGravityForce(Vector3 forceInput, float mass)
    {
        return forceInput + C.GravityVector * mass;
    }

    public static Vector3 AddDampingForce(Vector3 forceInput, Vector3 verletVelocity)
    {
        return forceInput + C.DAMPING * verletVelocity;
    }

    public static Vector3 GetDeltaVerletVelocity(Vector3 p1, Vector3 p1Last, Vector3 p2, Vector3 p2Last, float dt)
    {
        Vector3 v1 = VerletUtil.GetVerletVelocity(p1, p1Last, dt);
        Vector3 v2 = VerletUtil.GetVerletVelocity(p2, p2Last, dt);
        return v1 - v2;
    }

    public static float GetDragForceMagnitude(Vector3 velocity, float drag)
    {
        float mag = Vector3.Magnitude(velocity);
        return mag * mag * drag;
    }

    public static Vector3 GetDragVector(Vector3 velocity, float dragForceMagnitude)
    {
        return dragForceMagnitude * Vector3.Normalize(velocity);
    }

    public static Vector3 AddDragForce(Vector3 input, Vector3 verletVelocity, float drag)
    {
        float dragForceMag = GetDragForceMagnitude(verletVelocity, drag);
        return input - GetDragVector(verletVelocity, dragForceMag);
    }
}
