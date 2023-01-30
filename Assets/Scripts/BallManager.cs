using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton
    
    private static BallManager _instance;
    public static BallManager Instance => _instance;

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
  [SerializeField] Ball ballPrefab;
  [SerializeField] public float initialBallSpeed = 250f;
  Ball initialBall;
  Rigidbody2D initialBallRb;
  
  public List<Ball> Balls {get;set;}
 
  void Start() 
  {
    InitBall();
    initialBallRb = initialBall.GetComponent<Rigidbody2D>();
  }

  void Update() 
  {
    if(!GameManager.Instance.IsGameStarted)  
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall.transform.position = ballPosition;

        if (Input.GetMouseButtonDown(0))
        {
            initialBallRb.isKinematic = false;
            initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
            GameManager.Instance.IsGameStarted = true;
        }
    }
  }

  void InitBall()
  {
    Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
    Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
    initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
    
    this.Balls = new List<Ball>
    {
        initialBall
    };
  }
}
