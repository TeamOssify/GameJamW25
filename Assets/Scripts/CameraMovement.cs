using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour {
    private Camera targetCamera;

    [SerializeField]
    [Range(1, 12)]
    private float maxZoomIn = 1;

    [SerializeField]
    [Range(1, 12)]
    private float maxZoomOut = 12;

    [SerializeField]
    private float slowSpeed = 0.1f;

    [SerializeField]
    private float fastSpeed = 0.25f;

    [SerializeField]
    private Vector2 minPos = new(-10, -10);

    [SerializeField]
    private Vector2 maxPos = new(10, 10);

    private void Awake() {
        targetCamera = GetComponent<Camera>();
    }

    private void Update() {
        var deltaScroll = Mouse.current.scroll.ReadValue();
        targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize - deltaScroll.y, maxZoomIn, maxZoomOut);

        var fast = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);

        var deltaPos = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) deltaPos += Vector3.up;
        if (Input.GetKey(KeyCode.A)) deltaPos += Vector3.left;
        if (Input.GetKey(KeyCode.S)) deltaPos += Vector3.down;
        if (Input.GetKey(KeyCode.D)) deltaPos += Vector3.right;

        var speedModifier = fast ? fastSpeed : slowSpeed;
        transform.position += deltaPos * speedModifier;
    }
}