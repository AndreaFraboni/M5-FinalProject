using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] bool isOpen = false;

    private void Awake()
    {
        if (_anim == null) _anim = GetComponent<Animator>();
    }

    public void ActiveDoor()
    {
        if (isOpen)
        {
            _anim.SetTrigger("CloseDoor");
            isOpen = false;
        }
        else
        {
            _anim.SetTrigger("OpenDoor");
            isOpen = true;
        }
    }

}
