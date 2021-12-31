using UnityEngine;

public class CircleTangent : MonoBehaviour
{
    protected Vector3 getRotatedTangent(float degree, float radius)
    {
        double angle = degree * Mathf.Deg2Rad;
        float x = radius * (float)System.Math.Sin(angle);
        float z = radius * (float)System.Math.Cos(angle);
        return new Vector3(x, 0, z);
    }
    
    //A = center of outer circle
    //B = center of inner circle
    //C = center of tangent circle
    //AB = distance between outer circle center and inner circle center
    //AC = distance between outer circle center and tangent circle center
    //BC = distance between inner circle center and tangent circle center

    protected Vector4 findTangentCircle(Vector4 A, Vector4 B, float degree)
    {
        Vector3 C = getRotatedTangent(degree, A.w);
        float AB = Mathf.Max(Vector3.Distance(new Vector3(A.x, A.y, A.z), new Vector3(B.x, B.y, B.z)), 0.1f);
        float AC = Vector3.Distance(new Vector3(A.x, A.y, A.z), C);
        float BC = Vector3.Distance(new Vector3(B.x, B.y, B.z), C);
        float angleCAB = ((AB * AB) + (AC * AC) - (BC * BC)) / (2 * AB * AC);
        float r = (((A.w * A.w) - (B.w * B.w) + (AB * AB)) - (2*A.w*AB* angleCAB)) / (2 * (A.w + B.w - AB * angleCAB));
        C = getRotatedTangent(degree, A.w - r);
        return new Vector4(C.x, C.y, C.z, r);
    }
}
