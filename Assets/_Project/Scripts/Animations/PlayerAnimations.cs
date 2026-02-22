using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animator Params")]
    [SerializeField] private string _verticalSpeedParamName = "vSpeed";
    [SerializeField] private string _horizontalSpeedParamName = "hSpeed";
    [SerializeField] private string _walkBool = "isWalking";
    [SerializeField] private string _runBool = "isRunning";

    private PlayerController _pc;
    private Rigidbody _rb;
    private Animator _anim;

    private bool _wasGrounded;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _pc = GetComponentInParent<PlayerController>();
        _rb = _pc.GetComponent<Rigidbody>();
        _wasGrounded = _pc.isGrounded;
    }

    private void SetHorizontalSpeedParam(float dirx)
    {
        if (_anim) _anim.SetFloat(_horizontalSpeedParamName, dirx);
    }

    private void SetVerticalSpeedParam(float diry)
    {
        if (_anim) _anim.SetFloat(_verticalSpeedParamName, diry);
    }

    private void SetDirectionalSpeedParams(Vector2 direction)
    {
        SetHorizontalSpeedParam(direction.x);
        SetVerticalSpeedParam(direction.y);
    }
    public void SetBoolParam(string stateParam, bool value)
    {
        if (_anim) _anim.SetBool(stateParam, value);
    }

    public void SetTriggerParam(string stateParam)
    {
        if (_anim) _anim.SetTrigger(stateParam);
    }

    private void Update()
    {
        if (!_pc || !_anim || !_rb) return;
        Vector3 dir = _pc.GetDirection(); // prendi direzione del player
        bool moving = dir.magnitude > 0.001f; // si sta muovendo ????

        if (_pc.isGrounded) // sono sul terreno ???
        {
            if (moving)
            { // si muove !!
                if (_pc.isRunning) // Sta correndo ???
                {
                    _anim.SetBool(_runBool, true);
                    _anim.SetBool(_walkBool, false);
                }
                else
                { // non corre allora cammina
                    _anim.SetBool(_walkBool, true);
                    _anim.SetBool(_runBool, false);
                }

                _anim.SetFloat(_horizontalSpeedParamName, dir.x);
                _anim.SetFloat(_verticalSpeedParamName, dir.z);
            
            }
            else
            { // non si muove
                _anim.SetBool(_walkBool, false);
                _anim.SetBool(_runBool, false);
                _anim.SetFloat(_horizontalSpeedParamName, 0f);
                _anim.SetFloat(_verticalSpeedParamName, 0f);
            }
        }
    }
}