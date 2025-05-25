using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] private Transform _backGround;
    [SerializeField] private Transform _middleGround;
    [SerializeField] private Camera _playerCamera;

    // Update is called once per frame
    void Update()
    {
        _backGround.position = _playerCamera.transform.position / 2f;
        _middleGround.position = _playerCamera.transform.position / 4f;
    }
}
