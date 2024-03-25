using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagTimer : MonoBehaviour
{
    [SerializeField]
    float _timer = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer = _timer - 1;
        //need to subtract a second from a displayed timer number until the game is over
        // will probably make fixed
    }
}
