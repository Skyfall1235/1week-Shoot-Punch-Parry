using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class BasicMovement : NetworkBehaviour
{
    public Rigidbody rb;
    //define states on movmvent
    internal enum MovementState
    {
        Walk, Slide, Sprint, Strafe, None
    }
    //define a walk, sprint, and strafe speed
    public float walkSpeed, SprintSpeed, StrafeSpeed;
    //define a slide speed
    public float SlideSpeedBonus;
    //define a facing vector
    public Vector3 FacingDirection;
    public float DotProductAllowanceForDirectionalMapping = 0f;

    public bool isGrounded = false;
    public bool ShowDebugs = true;
    public Color DebugCameraColor = Color.white;
    public Color DebugGroundedState = Color.red;
    public Color DebugMovementDirection = Color.blue;
    public Color DebugSpecialMovement = Color.green;


    internal bool GroundedState;

    public void ApplyexplosionForceOnRigidbody(float forceValue, Vector3 origin)
    { 
    
    }
    public void ApplyExternalForceOnRigidbody(float forceValue, Vector3 direction)
    {

    }

    internal MovementState DetermineAcccelerationType(MovementActions currentAction)
    {
        //return MovementState.None;
        //this method will detemine whether or not, based on
        //based on the direction the camera is facing relative to the
        //use dot product for camera dir vs
        //will need to comput the direction of the camer and the player
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

    //bonus movment is a side strafe or sprint, slide is sperate as it will interrupt these
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
    //leave ready for inheritance

    //dbug
    //bool for debug lines

    //custom debug code

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
}
