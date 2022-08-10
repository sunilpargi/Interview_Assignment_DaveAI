using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{

    #region Variables
    private AudioSource m_footstepSound;
    private CharacterController m_characterController;

    [SerializeField] private AudioClip[] m_footstepClip;
    [SerializeField] private float m_accumulatedDistance;

    [HideInInspector]
    public float VolumeMin, VolumeMax;
    public float StepDistance;

    #endregion


    #region UnityCallBacks
    private void Awake()
    {
        m_footstepSound = GetComponent<AudioSource>();

        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckToPlayFootstepSound();
    }

    private void CheckToPlayFootstepSound()
    {

        // accumulated distance is the value how far can we go 
        // e.g. make a step or sprint, or move while crouching
        // until we play the footstep sound
        //accumulated_Distance += Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            m_accumulatedDistance += Time.deltaTime;

            if (m_accumulatedDistance > StepDistance)
            {
                m_footstepSound.volume = Random.Range(0.2f, 0.6f);
                m_footstepSound.clip = m_footstepClip[Random.Range(0, m_footstepClip.Length)];
                m_footstepSound.Play();

                m_accumulatedDistance = 0f;
            }
        }
        else
        {
            m_accumulatedDistance = 0f;

        }
    }

    #endregion
}
