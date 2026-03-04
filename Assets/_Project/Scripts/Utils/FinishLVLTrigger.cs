using UnityEngine;

public class FinishLVLTrigger : MonoBehaviour
{
    [Header("UI Manager")]
    [SerializeField] private GameUIManager _gameUIManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            _gameUIManager.Winner();
        }
    }

}
