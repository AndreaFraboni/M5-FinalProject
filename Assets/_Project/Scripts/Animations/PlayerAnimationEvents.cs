using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerController _pc;

    private void Awake()
    {
        if (_pc == null) _pc = GetComponentInParent<PlayerController>();
    }

    public void AE_FootStepSfx()
    {
        _pc.FootStepSound();
    }
}
