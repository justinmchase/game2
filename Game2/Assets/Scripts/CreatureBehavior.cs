using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
  public class CreatureBehavior : MonoBehaviour
  {

    public float Speed = 1.0f;

    public float IdleTime = 0.0f;
    public bool IsMoving = false;

    public bool IsMovingRight = false;
    public bool IsMovingLeft = false;

    public Vector3 MoveDirection = Vector3.zero;
    public bool IsRunning = false;

    private Rigidbody2D _rigidbody;

    public GameObject interactiveObject;

    public float StateTime = 0f;


    public void Start()
    {
      this._rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {

      this.StateTime += Time.fixedDeltaTime;

      this.IdleTime += Time.fixedDeltaTime;

      var animator = this.GetComponent<Animator>();

      float speed = this.Speed * (IsRunning ? 3 : 1);

      this.MoveDirection.Normalize();

      float moveY =  MoveDirection.x * speed * Time.fixedDeltaTime;
      float moveX = MoveDirection.y * speed * Time.fixedDeltaTime;

      Vector3 velocity = new Vector3(moveX, moveY, 0);
      Vector3 moveDir = velocity;
      if (moveDir.magnitude != 0)
      {
        moveDir.Normalize();
        this.IdleTime = 0;
      }

      if (moveDir.x < 0)
      {
        this.transform.localScale = new Vector3(-1, 1, 1);
      }

      if (moveDir.x > 0)
      {
        this.transform.localScale = new Vector3(1, 1, 1);
      }

      animator.SetFloat("MoveX", moveDir.x);
      animator.SetFloat("MoveY", moveDir.y);

      this.IsMoving = moveDir.magnitude > 0;
      animator.SetBool("IsMoving", this.IsMoving);
      animator.SetFloat("IdleTime", this.IdleTime);

      // this.transform.position += velocity;

      var p = this.transform.position + velocity;
      this._rigidbody.MovePosition(p);
    }

    public void Interact(bool engage)
    {
      if (this.interactiveObject != null)
      {
        this.interactiveObject.SendMessage("Interact", engage);
      }
    }
  }
}
