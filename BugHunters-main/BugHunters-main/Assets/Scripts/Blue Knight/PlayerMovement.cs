using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    public Vector2 addVector;

    float time = 0;
    float lastTime = 0;

    private void Update()
    {
        while (lastTime < time)
        {
            lastTime += 0.005f;
            addVector *= 0.98f;
        }

        time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector2 movementVector = inputVector * (_moveSpeed + PlayerStats.Instance.Speed/10f) * Time.fixedDeltaTime + addVector;
        _rb.MovePosition(_rb.position + movementVector);

        _animator.SetFloat("Speed", inputVector.sqrMagnitude);
        if (inputVector.magnitude > 0.01f)
        {
            if (!Camera.main.GetComponent<AudioSource>().isPlaying) Camera.main.GetComponent<AudioSource>().UnPause();
            _animator.SetFloat("Horizontal", inputVector.x);
            _animator.SetFloat("Vertical", inputVector.y);
        }
        else
        {
            if (Camera.main.GetComponent<AudioSource>().isPlaying) Camera.main.GetComponent<AudioSource>().Pause();
        }
    }
}
