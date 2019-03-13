using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Teams
{
    Player,
    Monster,
    Neutral
};

public class CreatureBehavior : MonoBehaviour
{
    private static readonly Vector3 CenterOffset = new Vector3(0.5f, 0.5f, 0.0f);

    public float Speed = 1.0f;

    public float IdleTime = 0.0f;
    public bool IsMoving = false;

    public bool IsMovingRight = false;
    public bool IsMovingLeft = false;

    public Vector3Int? Target;
    public Vector3Int[] Path;
    public Vector3 MoveDirection = Vector3.zero;
    public bool IsRunning = false;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;

    public GameObject interactiveObject;

    public float StateTime = 0f;
    
    public Teams Team = Teams.Monster;

    public GameObject AttackFocus = null;


    public void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody2D>();
        this._collider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        this.StateTime += Time.fixedDeltaTime;
        this.IdleTime += Time.fixedDeltaTime;

        var animator = this.GetComponent<Animator>();

        var speed = this.Speed * (IsRunning ? 3 : 1);

        this.FollowPath();

        var d = MoveDirection.normalized;
        var moveY = d.y * speed * Time.fixedDeltaTime;
        var moveX = d.x * speed * Time.fixedDeltaTime;

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
    
    public void Attack()
    {
        if (this.AttackFocus != null)
        {
            // todo: deal damage...
        }
    }

    public void SetTarget(Vector3Int? target)
    {
        if (target == null)
        {
            this.MoveDirection = Vector3.zero;
            this.Path = null;
        }

        var t = this.Target;
        this.Target = target;
        if (t != target)
        {
            this.GetTargetPath();
        }
    }

    private void GetTargetPath()
    {
        if (this.Target != null)
        {
            var colliderOffset = new Vector3(this._collider.offset.x, this._collider.offset.y, 0);
            var p = this.transform.position + colliderOffset;
            var game = GameManager.current;
            var level = game.Level;
            var path = Game.Algorithms.AStar.GetPath(
                level.ctx.OpenTiles,
                level.ctx.ObstructedTiles,
                Vector3Int.FloorToInt(p),
                this.Target.Value);

            this.Path = path;
        }
    }

    private void FollowPath()
    {
        if (this.Path != null && this.Path.Length > 0)
        {
            var colliderOffset = new Vector3(this._collider.offset.x, this._collider.offset.y, 0);
            var p = this.transform.position + colliderOffset;

            for (var i = 0; i < this.Path.Length; i++)
            {
                var pi = this.Path[i];
                DrawRectAt(pi + CenterOffset, 0.5f, Color.magenta);
                DrawRectAt(pi + CenterOffset, 0.05f, Color.yellow);
            }

            DrawRectAt(p, .25f, Color.yellow);

            var p0 = this.Path[0] + CenterOffset;
            if ((this.MoveDirection != Vector3.zero && Vector3.Dot(this.MoveDirection, p0 - p) <= 0.0f)
                || Vector3.Distance(p0, p) < 0.1f)
            {
                this.Path = this.Path.Skip(1).ToArray();
            }

            if (this.Path.Length < 2)
            {
                this.MoveDirection = Vector3.zero;
                this.Path = null;
                return;
            }

            p0 = this.Path[0] + CenterOffset;
            var dir = p0 - p;
            this.MoveDirection = dir;

            Debug.DrawLine(p, p0, Color.green);
            Debug.DrawLine(p, p + this.MoveDirection, Color.red);
        }
    }

    private static void DrawRectAt(Vector3 p, float size, Color color)
    {
        var ll = p + new Vector3(-size, -size, 0);
        var ul = p + new Vector3(-size, size, 0);
        var ur = p + new Vector3(size, size, 0);
        var lr = p + new Vector3(size, -size, 0);
        Debug.DrawLine(ll, ul, color);
        Debug.DrawLine(ul, ur, color);
        Debug.DrawLine(ur, lr, color);
        Debug.DrawLine(lr, ll, color);
    }
}
