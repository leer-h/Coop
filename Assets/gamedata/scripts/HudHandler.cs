using UnityEngine;

public class HudHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0.2f, -0.2f, 0.5f);
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;

    [Header("Sway Settings")]
    [SerializeField] private float swayAmount = 0.05f;
    [SerializeField] private float swayRotation = 2f;
    [SerializeField] private float swaySmooth = 6f;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private Vector3 swayPosOffset;
    private Quaternion swayRotOffset;

    void LateUpdate()
    {
        targetPos = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
        targetRot = cameraTransform.rotation * Quaternion.Euler(rotationOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 targetSwayPos = new Vector3(-mouseX, -mouseY, 0) * swayAmount;
        swayPosOffset = Vector3.Lerp(swayPosOffset, targetSwayPos, Time.deltaTime * swaySmooth);

        Vector3 targetSwayRot = new Vector3(mouseY, -mouseX, 0) * swayRotation;
        swayRotOffset = Quaternion.Slerp(swayRotOffset, Quaternion.Euler(targetSwayRot), Time.deltaTime * swaySmooth);

        transform.localPosition += swayPosOffset;
        transform.localRotation *= swayRotOffset;
    }
}
