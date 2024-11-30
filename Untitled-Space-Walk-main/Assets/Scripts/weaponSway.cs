using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    #region Rotation

    [Header("Sway rotation settings")]
    [Tooltip("This is how fast the intial ROTATION response is to a turn. Turn this down for heavier weapons.")]
    [SerializeField]
    private float initialRotationSwingSpeed = 10f;
    public float InitialRotationSwingSpeed
    {
        get { return initialRotationSwingSpeed; }
        set { initialRotationSwingSpeed = value; }
    }

    [Tooltip("How fast the gun returns ROTATION to zero point after being swung. Turn this down for heavier weapons.")]
    [SerializeField]
    private float returnRotationSwingSpeed = 5f;
    public float ReturnRotationSwingSpeed
    {
        get { return returnRotationSwingSpeed; }
        set { returnRotationSwingSpeed = value; }
    }


    [Tooltip("This is the up and down/left and right tilt amount.")]
    [SerializeField]
    private float linearRotationalSwayAmount = 20f;
    public float LinearRotationSwayAmount
    {
        get { return linearRotationalSwayAmount; }
        set { linearRotationalSwayAmount = value; }
    }


    [Tooltip("This is the rotational sway along the forward axis that matches left and right movement.")]
    [SerializeField]
    private float forwardRotationSwayAmount = 30f;
    public float ForwardRotationSwayAmount
    {
        get { return forwardRotationSwayAmount; }
        set { forwardRotationSwayAmount = value; }
    }

    #endregion



    #region Position

    [Header("Sway position settings")]
    [Tooltip("This is how fast the MOVEMENT response is to a turn. Turn this down for heavier weapons.")]
    [SerializeField]
    private float movementSwaySpeed = 3f;
    public float MovementSwaySpeed
    {
        get { return movementSwaySpeed; }
        set { movementSwaySpeed = value; }
    }

    [Tooltip("This is the up and down movement amount.")]
    [SerializeField]
    private float verticalSwayAmount = 1f;
    public float VerticalSwayAmount
    {
        get { return verticalSwayAmount; }
        set { verticalSwayAmount = value; }
    }


    [Tooltip("This is the left and right movement amount.")]
    [SerializeField]
    private float horizontalSwayAmount = 1f;
    public float HorizontalSwayAmount
    {
        get { return horizontalSwayAmount; }
        set { horizontalSwayAmount = value; }
    }

    #endregion

    #region Private stores

    private Quaternion targetRotation; //Frame to frame rotation save
    private Quaternion startingLocalRotation; //Local rotation on start
    private Vector3 startingLocalPosition; //Local position on start
    private float horizontalPos; //Frame to frame position save
    private float verticalPos; //Frame to frame position save

    private float mouseX;
    private float mouseY;

    #endregion

    private void Start()
    {
        startingLocalPosition = transform.localPosition;
        startingLocalRotation = transform.localRotation;            
    }

    void Update()
    {
        GetMouseInput();
        ApplyRotationLerp();
        ApplyMovementLerp();
    }

    private void ApplyRotationLerp()
    {
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY * LinearRotationSwayAmount, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX * LinearRotationSwayAmount, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(-mouseX * ForwardRotationSwayAmount, Vector3.forward);

        Quaternion frameTargetRot = rotationX * rotationY * rotationZ;

        targetRotation = Quaternion.Slerp(targetRotation, frameTargetRot, Time.deltaTime * ReturnRotationSwingSpeed);


        transform.localRotation = Quaternion.Slerp(startingLocalRotation, targetRotation, InitialRotationSwingSpeed * Time.deltaTime);
    }

    private void ApplyMovementLerp()
    {
        horizontalPos = Mathf.Lerp(horizontalPos, mouseX * horizontalSwayAmount, movementSwaySpeed * Time.deltaTime);
        verticalPos = Mathf.Lerp(verticalPos, mouseY * verticalSwayAmount, movementSwaySpeed * Time.deltaTime);

        Vector3 targetPosition = new Vector3(horizontalPos, verticalPos, 0);

        transform.localPosition = targetPosition + startingLocalPosition;
    }

    private void GetMouseInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}
