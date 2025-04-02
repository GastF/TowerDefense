using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        RotateTowardsCamera();
    }
    private void RotateTowardsCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
