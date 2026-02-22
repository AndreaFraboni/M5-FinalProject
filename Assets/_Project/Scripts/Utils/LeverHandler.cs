using UnityEngine;
using UnityEngine.Events;

public class LeverHandler : MonoBehaviour
{
    [SerializeField] Animator _anim;

    public GameObject _eCanvas;

    public UnityEvent OnLeverMoved;

    public bool isDown = false;

    [SerializeField] private bool isPlayerInside = false;

    private void Awake()
    {
        if (_anim == null) _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isPlayerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isDown)
            {
                _anim.SetTrigger("LeverUp");
                isDown = false;
            }
            else
            {
                _anim.SetTrigger("LeverDown");
                isDown = true;
            }

            OnLeverMoved.Invoke();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        _eCanvas.SetActive(true);
        isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _eCanvas.SetActive(false);
        isPlayerInside = false;
    }
}
