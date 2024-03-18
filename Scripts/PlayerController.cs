using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //this makes it to where this script depends on having the Rigibody component 
public class PlayerController : MonoBehaviour
{

    //these are the attributes for _speed
   [SerializeField] //this allows a private variable to still be accessed and changed inside of unity
   [Min(0)] //this makes minimum speed 0
   [TooltipAttribute("Player Speed")] //this gives an explanation of what this is when you hover over it in the unity inspector
   private float _speed = 25; //= 25 will make it a default value in the inspector
   private float _maxSpeed;
   private float _acceleration;
   private Vector3 _moveDirection;

    [SerializeField, Tooltip("How much vertical force is applied when jumping")] //you can declare attributes like this or like above either is correct
    private float _jumpForce = 25;

    private Rigidbody _rigidbody;

    /// <summary>
    /// checks to see if the player is on the ground and can jump or not.
    /// </summary>
    private bool _isGrounded = false;

    public float Speed
    {
        get => _speed;                      //this is the same as get = {return _speed} apparently is called syntatic sugar and something to called lamda syntax?
        set =>_speed = Mathf.Max(0, value); //Mathf.Max insures this value can never be set to a nagative value.
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
       _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        _isGrounded = true;
    }


    private void FixedUpdate() //this is something all monobehaviours have it. Runs your physics independent of your frame right.
    {
        //add movement force
        _rigidbody.AddForce(_moveDirection * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);


        if (_rigidbody.velocity.magnitude > _maxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
       
    }
}
