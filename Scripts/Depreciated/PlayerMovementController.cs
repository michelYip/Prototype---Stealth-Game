using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Exposed

    [SerializeField] private float moveSpeed = 3f;

    #endregion

    #region Unity API
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 MovementInput = GetMovementAxis();
        Vector3 moveDirectionX = new Vector3(MovementInput.x,0,0);
        Vector3 moveDirectionZ = new Vector3(0,0, MovementInput.z);
        _rigidBody.AddForce(moveDirectionX * moveSpeed);
        _rigidBody.AddForce(moveDirectionZ * moveSpeed);

        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, moveSpeed);
        
        Debug.Log(MovementInput.x + " | " + Vector3.SignedAngle(moveDirectionX, transform.right, Vector3.up));
        
        Vector3 mouseMovement = GetMouseAxis();
        //_rigidBody.MoveRotation(Quaternion.LookRotation(_cameraTransform.forward, Vector3.up));
        transform.Rotate(0, mouseMovement.x, 0);

        //Debug.Log(GetMouseAxis());
    }

	private void FixedUpdate()
	{
	}

	#endregion

	#region Main Methods

    public Vector3 GetMovementAxis()
	{
        return Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1f);
	}

    public Vector3 GetMouseAxis()
	{
        return new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
	}

    public Vector3 GetLookAtAxis()
	{
        return Vector3.zero;
	}

	#endregion

	#region Privates

	private Rigidbody _rigidBody;
    private Transform _cameraTransform;

    #endregion
}
