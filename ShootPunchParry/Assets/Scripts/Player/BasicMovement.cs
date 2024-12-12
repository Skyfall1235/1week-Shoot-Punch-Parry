using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class BasicMovement : NetworkBehaviour
{
    public Rigidbody rb;
    //define states on movmvent
    
    //define a walk, sprint, and strafe speed
    [Header("Variable base speed of player")]
    public float walkSpeed, SprintSpeed, StrafeSpeed;
    //define a slide speed
    public float SlideSpeedBonus;
    //define a facing vector
    [Header("Camera To direction mapping")]
    public Vector3 FacingDirection;
    public float DotProductAllowanceForDirectionalMapping = 0f;

    public bool isGrounded = false;
    [Header("Debugs")]
    public bool ShowDebugs = true;
    public Color DebugCameraColor = Color.white;
    public Color DebugGroundedState = Color.red;
    public Color DebugMovementDirection = Color.blue;
    public Color DebugSpecialMovement = Color.green;

    #region Enablisms
    public void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnDisable()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            //diable the controller if we arent owner
            this.enabled = false;
        }
    }

    #endregion

    #region Application of forces on this object
    public void ApplyexplosionForceOnRigidbody(float forceValue, Vector3 origin)
    { 
    
    }
    public void ApplyExternalForceOnRigidbody(float forceValue, Vector3 direction)
    {

    }

    #endregion

    internal MovementState DetermineAcccelerationType(MovementActions currentAction)
    {
        Vector3 dirOfPlayerTravel = Vector3.zero;
        Vector3 DirOfCamera = FacingDirection.normalized;
        float DotOfCameraAndMovement = Vector2.Dot(DirOfCamera, dirOfPlayerTravel);
        if(DotOfCameraAndMovement > DotProductAllowanceForDirectionalMapping && currentAction.IsattemptingBonusMovement) 
        {
            return MovementState.Strafe;
        }
        else if(currentAction.IsattemptingBonusMovement)
        {
            return MovementState.Sprint;
        }
        else if(dirOfPlayerTravel !=  Vector3.zero)
        {
            return MovementState.Walk;
        }
        else return MovementState.None;
    }


    //methods for moving and managing speed
    internal void MovePlayer(MovementActions inputs, Vector3 FacingDirection)
    {
        //use the values we have and the direction the player is giving
    }

    //bonus movement is a side strafe or sprint, slide is sperate as it will interrupt these
    internal MovementActions ConvertInputToUsableVector(InputActionProperty moveAction, InputActionProperty bonusMovement, InputActionProperty attemptSlide)
    {
        Vector2 movementInput = RetrieveValueFromAction<Vector2>(moveAction);
        bool isUsingBonusMovement = RetrieveValueFromAction<bool>(bonusMovement);
        bool isAttemptingSlide = RetrieveValueFromAction<bool>(attemptSlide);
        if (isAttemptingSlide)
        {
            isUsingBonusMovement = false;
        }
        MovementActions actions = new MovementActions(movementInput, isUsingBonusMovement, isAttemptingSlide);
        return actions;
    }

    /// <summary>
    /// Retrieves a value of type T from a Unity InputActionProperty.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve. Must be a struct.</typeparam>
    /// <param name="actionProperty">The InputActionProperty object containing the value.</param>
    /// <returns>The value of type T retrieved from the InputActionProperty.</returns>
    static public T RetrieveValueFromAction<T>(InputActionProperty actionProperty) where T : struct
    {
        // retrive the value from the action map
        T actionValue = actionProperty.action.ReadValue<T>();
        //return the value
        return actionValue;
    }

    #region Collision

    internal virtual void OnCollisionEnter(Collision collision)
    {
        //check collisions for ground
        switch (collision.gameObject.layer)
        {
            case 6: 

                break;
        }
    }
    internal virtual void OnCollisionExit(Collision collision)
    {
        //check collisions for ground
        switch (collision.gameObject.layer)
        {
            case 6: 

                break;
        }
    }

    #endregion

    //debugs missing special movement
    private void OnDrawGizmos()
    {
        if(ShowDebugs)
        {
            //camera
            Vector3 CameraStartLocation = Camera.main.transform.position;
            Vector3 CameraLookDirection = Camera.main.transform.forward;
            Debug.DrawLine(CameraStartLocation, CameraLookDirection, DebugCameraColor);
            //grounded state
            Vector3 footPlacement = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            Gizmos.DrawSphere(footPlacement, 0.2f);
            //movement
            Debug.DrawLine(transform.position + rb.linearVelocity.normalized, transform.position, DebugMovementDirection);
            //special movement
        }
    }

    internal struct MovementActions
    {
        public Vector2 XYaxis;
        public bool IsattemptingBonusMovement;
        public bool IsAttemptingSlide;
        public MovementActions(Vector2 xYaxis, bool isattemptingBonusMovement, bool isAttemptingSlide)
        {
            XYaxis = xYaxis;
            IsattemptingBonusMovement = isattemptingBonusMovement;
            IsAttemptingSlide = isAttemptingSlide;
        }
    }

    internal enum MovementState
    {
        Walk, Slide, Sprint, Strafe, None
    }
    
}
