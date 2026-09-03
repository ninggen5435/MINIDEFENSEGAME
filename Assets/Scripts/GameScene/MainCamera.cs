using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject MainCam;
    private Transform CameraTransform;
    private float MoveSpeed = 2f;
    private Vector3 MoveVec2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraTransform = MainCam.transform;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if(CameraTransform.position.x >= 3.2f)
        {
            CameraTransform.position = new Vector3(3.2f, CameraTransform.position.y, CameraTransform.position.z);
        }
        else if (CameraTransform.position.x <= -3.2f)
        {
            CameraTransform.position = new Vector3(-3.2f, CameraTransform.position.y, CameraTransform.position.z);
        }
        float horizontal = Input.GetAxis("Horizontal");
        MoveVec2 = new Vector3(horizontal, 0, 0).normalized;
        transform.position += MoveVec2 * MoveSpeed * Time.deltaTime;
    }
}
