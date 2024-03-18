using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //this makes it to where this script depends on having the Rigibody component 
public class PlayerController : MonoBehaviour
{





    //these are the attributes for _speed
   [SerializeField] //this allows a private variable to still be accessed and changed inside of unity
   [TooltipAttribute("Player Max Speed")] //this gives an explanation of what this is when you hover over it in the unity inspector
   private float _maxSpeed;
   [SerializeField]
   private float _acceleration;
    [SerializeField]
    private float _jumpHeight;
    [Space]
    [SerializeField]
    private Vector3 _groundCheck;
    [SerializeField]
    private float _groundCheckRadius;


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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   ///old code for movement and jumping 
       // float moveInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw snaps to one or negative one without any smooth build up. A and D, Left arrow and Right arrow.
       // float jumpInput = 0;                              //GetAxis will build up to 1 or -1 to essentially accelorate up to it.

       // if (Input.GetKeyDown(KeyCode.Space) && _isGrounded == true)
       // {
        //    jumpInput = 1;
        //    _isGrounded = false;
       // }
        //_rigidbody.AddForce(Vector3.right * moveInput * _speed * Time.deltaTime);
       // _rigidbody.AddForce(Vector3.up * jumpInput * _jumpForce * Time.deltaTime, ForceMode.Impulse);

        //Get Movement Input
       _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        _jumpInput = Input.GetAxisRaw("Jump") != 0;
        

    }

    //private void OnTriggerEnter(Collider other)
   // {
  //      _isGrounded = true;
  //  }
   


    private void FixedUpdate() //this is something all monobehaviours have it. Runs your physics independent of your frame right.
    {   //ground check
        _isGrounded = Physics.OverlapSphere(transform.position + _groundCheck, _groundCheckRadius).Length > 1;

        //add movement force
        _rigidbody.AddForce(_moveDirection * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);


            //Clamp velocity to _maxSpeed
            Vector3 velocity = _rigidbody.velocity;
            float newXSpeed = Mathf.Clamp(_rigidbody.velocity.x, - _maxSpeed, _maxSpeed);
            velocity.x = newXSpeed;
            _rigidbody.velocity = velocity;
  

        if (_jumpInput && _isGrounded)
        {
            //Calculate force needed to reach _jumpHeight
            float force = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _groundCheck, _groundCheckRadius);
    }
#endif
}
