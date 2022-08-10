using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private float m_speed, m_sprintModifier, m_normalSpeed;
    [SerializeField] private Transform m_cam;
    [SerializeField] private float m_turnSomoothTime = 0.1f;
    [SerializeField] private Animator m_anim;

    private float m_turnSmoothVelocity;
    private PlayerFootSteps m_playerFootSteps;

    #endregion


    #region UnityCallBacks
    private void Start()
    {
        Cursor.visible = false;
        m_playerFootSteps = GetComponent<PlayerFootSteps>();
        m_anim = GetComponent<Animator>();
    }
  
    private void Update()
    {
        //Player input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
    
        //Normalized Makes direction vector unit length
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //checking length of the direction vector
        if (direction.magnitude >= 0.1f)
        {
            //To find the rotation based on player direction vector gives in radian angle 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_cam.eulerAngles.y;

            // To smooth the transition between angles
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_turnSmoothVelocity, m_turnSomoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            if (vertical == 1 && isSprinting)
            {
                   m_speed = m_sprintModifier;
                   m_playerFootSteps.StepDistance = 0.4f;
            }
            else
            {               
                m_speed = m_normalSpeed;
            }

            //TO move player in the direction of camera, it gives direction where we want to move
            //Changes rotation into direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            moveDir.y = -1f;


            m_characterController.Move(moveDir.normalized * m_speed * Time.deltaTime);
        }

        //Player Animation
        if (horizontal != 0 || vertical != 0)
        {
            if (!isSprinting)
            {
                m_playerFootSteps.StepDistance = 0.6f;
            }
          
            m_anim.SetBool("Walk", true);
        }
        else
        {
            m_anim.SetBool("Walk", false);         
        }
    }

    #endregion
}
