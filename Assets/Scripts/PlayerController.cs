using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //this makes it to where this script depends on having the Rigibody component 
public class PlayerController : MonoBehaviour  //nearly all Classes in unity inherit from MonoBehaviour or inherit from something else that inherits from MonoBehaviour
{
    [SerializeField]
    private bool _isPlayerOne = true;




    //these are the attributes for _maxSpeed
   [SerializeField] //this allows a private variable to still be accessed and changed inside of unity inspector
   [TooltipAttribute("Player Max Speed")] //this gives an explanation of what this is when you hover over it in the unity inspector
   private float _maxSpeed = 2; //giving the = 2 basically gives a default value of unity units per second of movment.

   [SerializeField]
   private float _acceleration = 100;

    [SerializeField]
    private float _jumpHeight = 2;

    [Space] //puts a space in the inspector where this variable is.
    [SerializeField]
    private Vector3 _groundCheckPosition =  new Vector3();

    [SerializeField]
    private float _groundCheckRadius = 0.45f;


   private Vector3 _moveDirection;
    

    private Rigidbody _rigidbody;

    /// <summary>
    /// checks to see if the player is on the ground and can jump or not.
    /// </summary>
    private bool _isGrounded = false;
    private bool _jumpInput = false;

    public float MaxSpeed
    {
        get => _maxSpeed;                      //this is the same as get = {return _speed} apparently is called syntatic sugar and something to called lamda syntax?
        set =>_maxSpeed = Mathf.Max(0, value); //Mathf.Max insures this value can never be set to a nagative value.
    }


    private void Awake() //should never do anything except act as a constructor for your scripts it runs before everything run time.
    {                   //All gameobjects are essentially set up in the scene during Awake so never reference another object in Awake besides itself.
      _rigidbody = GetComponent<Rigidbody>();

            // if (_rigidbody == null)
           // Debug.LogError("Rigibody is null!"); //this insures that if rigibody is ever null it will break and present a log error

        Debug.Assert(_rigidbody != null, "Rigibody is null!");  // this Debug.Assert functions the same way as the if statemeny above.

        //Assert's is a debugging tool for unity but it is not necessary
    }


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
  private void Update()
    {
        //GetAxisRaw snaps to one or negative one without any smooth build up. A and D, Left arrow and Right arrow.
        //GetAxis will build up to 1 or -1 to essentially accelorate up to it.


        if (_isPlayerOne)
        {
            //Get Movement Input
            _moveDirection = new Vector3(Input.GetAxisRaw("Player1Horizontal"), 0, 0);
            _jumpInput = Input.GetAxisRaw("Player1Jump") != 0; //this statement essentially functions as we want _jumpInput to be true if Input.GetAxisRaw("Jump") != 0 and false if the opposite.
        }
        else
        {
            _moveDirection = new Vector3(Input.GetAxisRaw("Player2Horizontal"), 0, 0);
            _jumpInput = Input.GetAxisRaw("Player2Jump") != 0;
        }
            

    }
    
    /// old code for using OnTriggerEnter() function for if _isGrounded is true.
     //private void OnTriggerEnter(Collider other)
    // {
   //      _isGrounded = true;
  //   }
   


    private void FixedUpdate() //this is something all monobehaviours have it. Runs your physics independent of your frame right.
    {   //ground check
        _isGrounded = Physics.OverlapSphere(transform.position + _groundCheckPosition, _groundCheckRadius).Length > 1;

        //add movement force
        Vector3 force = _moveDirection * _acceleration * Time.fixedDeltaTime;
        _rigidbody.AddForce(force, ForceMode.VelocityChange);


        //Clamp velocity to _maxSpeed
        Vector3 velocity = _rigidbody.velocity;
        velocity.x = Mathf.Clamp(_rigidbody.velocity.x, -_maxSpeed, _maxSpeed);
        _rigidbody.velocity = velocity;
        
        // Might as Well Jump
        if (_jumpInput && _isGrounded)
        {
            //Calculate force needed to reach _jumpHeight in unity units
            float jumpForce = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y); 
            //take the up vector for the rigidbody and multiply it by jumpforce
            //then the force impulse means basically you want to add that force all at once in an impulse.
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //ALERT THIS WILL ONLY RUN WHEN WE ARE IN THE UNITY EDITOR. 
    //the #if UNITY_EDITOR and #endif means it will be ignored by the compiler in any build that isn't building in the Unity Editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //This will draw the ground check sphere.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _groundCheckPosition, _groundCheckRadius);
    }
#endif
}
