using UnityEngine;

public class Spinny : MonoBehaviour
{
    private void FixedUpdate()
        => transform.rotation *= Quaternion.AngleAxis(2f, Vector3.up);
}
