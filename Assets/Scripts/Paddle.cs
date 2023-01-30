using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton
    
    private static Paddle _instance;
    public static Paddle Instance => _instance;

    void Awake() 
    {
        if (_instance != null)    
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    #endregion
    Camera mainCamera;
    SpriteRenderer spriteRenderer;
    float defaultLeftClamp = 135;
    float defaultRightClamp = 410;
    
    float paddleInitialY;
    float defaultPaddeWidthInPixels = 200;

    void Start() 
    {
        mainCamera = FindObjectOfType<Camera>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        paddleInitialY = this.transform.position.y;
    }
    
    void Update()
    {
        PaddleMovement();
    }

    void PaddleMovement()
    {
        float paddleShift = (defaultPaddeWidthInPixels - ((defaultPaddeWidthInPixels / 2) * this.spriteRenderer.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWoldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels,0,0)).x;
        this.transform.position = new Vector3(mousePositionWoldX, paddleInitialY, 0);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitpoint = other.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitpoint.x;

            if (hitpoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-Mathf.Abs(difference * 200), BallManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2(Mathf.Abs(difference * 200), BallManager.Instance.initialBallSpeed));
            }
        }
    }
}
