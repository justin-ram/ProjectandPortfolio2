using UnityEngine;

public class camerController : MonoBehaviour
{
    [SerializeField] int camSens;
    [SerializeField] int lockVertMin, lockVertMax;

    float camRotX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rotateCamera();
    }

    void rotateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * camSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * camSens;

        camRotX -= mouseY;
        camRotX = Mathf.Clamp(camRotX, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(camRotX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
