using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BallisticsTools
{
    /// <summary>
    /// Calculates the velocity to give to a rigidbody to go from origin to target
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }
}
