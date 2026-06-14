using UnityEngine;
using UnityEngine.InputSystem;

public class newInputSystemMovement : MonoBehaviour
{
    private Rigidbody sphereRigidbody;

    void Awake()
    {
        sphereRigidbody = GetComponent<Rigidbody>();
    }

}
