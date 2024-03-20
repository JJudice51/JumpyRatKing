using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagBehavior : MonoBehaviour
{
    [SerializeField]
    private bool _isTagged = false;

    [SerializeField]
    private ParticleSystem _taggedParticles;
    private bool _canBeTagged = true;
    public bool IsTagged { get => _isTagged; }
    public ParticleSystem TaggedParticles { get => _taggedParticles;  }
    public bool Tag()
    {   
        //if can't be tagged return false.
        if (!_canBeTagged) return false;
        
        //set that we're tagged
        _isTagged = true;
        _canBeTagged = false;

        //turn our trail renderer on.
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail == null) return false;

        trail.enabled = true;

        //run particle burst 
        TaggedParticles.Play();
        return true;
    }

    private void SetCanBeTagged()
    {
        _canBeTagged = true;
    }

    private void Start()
    {
        // Get my trail renderer
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail == null) return;

        //If I am tagged, then turn trail on, else off.
        if (IsTagged)
            trail.enabled = true;

        else
            trail.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If we aren't it do nothing
        if (!IsTagged)
            return;

        //Attempt to get the PlayerTagBahvior form what we hit
        PlayerTagBehavior tagBehavior = collision.gameObject.GetComponent<PlayerTagBehavior>();

        //If it didnt have on, return
        if (tagBehavior == null) return;

        //Tag the other player
        if (!tagBehavior.Tag()) return;

        //Set ourselves as not it
        _isTagged = false;
        _canBeTagged = false;
      
        //Turn off our trail renderer 
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;


        }
        //this below functions the same as above but uses an out variable
        /// if (TryGetComponent(out TrailRenderer trail))
        /// {
        ///   trail.enabled = false;
        /// }
    }
    

    private void OnCollisionExit(Collision collision)
    {
        Invoke("SetCanBeTagged", 0.5f);
    }












}
