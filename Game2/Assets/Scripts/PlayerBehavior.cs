using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Actors;

namespace Game
{
  public class PlayerBehavior : MonoBehaviour
  {

    public float Speed = 1.0f;

    public float IdleTime = 0.0f;
    public bool IsMoving = false;

    public bool IsMovingRight = false;
    public bool IsMovingLeft = false;

    private Rigidbody2D _rigidbody;
    private Player player;

    public void Start()
    {
      this._rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void Model(Player player)
    {
      this.player = player;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {

      this.IdleTime += Time.fixedDeltaTime;

      var animator = this.GetComponent<Animator>();

      float speed = this.Speed * (Input.GetKey(KeyCode.LeftShift) ? 3 : 1);

      float moveY = Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;
      float moveX = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;

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

      if (this.player != null) this.player.position = this.transform.position;
    }
  }
}
