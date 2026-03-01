using UnityEngine;

public class FinishLVLTrigger : MonoBehaviour
{
    [Header("UI Manager")]
    [SerializeField] private GameUIManager _gameUIManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            _gameUIManager.Winner();
        }
    }

}
