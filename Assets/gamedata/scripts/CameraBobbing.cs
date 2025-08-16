using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private float _bobbingSpeed = 0.18f;
    [SerializeField] private float _bobbingAmount = 0.2f;
    [SerializeField] private float _midpoint = 2.0f;
    [SerializeField] private float _horizontalBobbingAmount = 0.1f;
    [SerializeField] private float _horizontalBobbingSpeed = 0.1f;

    private float timer = 0.0f;
    private float bobbingValue = 0.0f;
    private float horizontalTimer = 0.0f;
    private float horizontalBobbingValue = 0.0f;

    [SerializeField] private CharacterMovement characterMovement;

    private void Update()
    {
        if (!characterMovement.IsGrounded()) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
            horizontalTimer = 0.0f;
        }
        else
        {
            float waveslice = Mathf.Sin(timer);
            timer += _bobbingSpeed * Time.deltaTime;
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }

            float horizontalWaveslice = Mathf.Sin(horizontalTimer);
            horizontalTimer += _horizontalBobbingSpeed * Time.deltaTime;
            if (horizontalTimer > Mathf.PI * 2)
            {
                horizontalTimer -= Mathf.PI * 2;
            }

            if (waveslice != 0)
            {
                bobbingValue = waveslice * _bobbingAmount;
            }
            else
            {
                bobbingValue = 0;
            }

            if (horizontalWaveslice != 0)
            {
                horizontalBobbingValue = horizontalWaveslice * _horizontalBobbingAmount;
            }
            else
            {
                horizontalBobbingValue = 0;
            }
        }

        Vector3 cameraPosition = transform.localPosition;
        cameraPosition.y = _midpoint + bobbingValue;
        cameraPosition.x = horizontalBobbingValue;
        transform.localPosition = cameraPosition;
    }
}