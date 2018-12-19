using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

		public static Vector3 XY(this Vector3 v){
			return new Vector3(v.x, v.y, 0f);
		}
}

public class ChasePlayer : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    private static readonly Vector3 CenterOffset = new Vector3(0.5f, 0.5f, 0.0f);

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		var creature = animator.GetComponent<CreatureBehavior>();
		if(creature != null)
        {
            var collider = creature.GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                var colliderOffset = new Vector3(collider.offset.x, collider.offset.y, 0);
                var p = creature.transform.position + colliderOffset;
                var game = GameManager.current;
                var level = game.Level;
                var path = Game.Algorithms.AStar.GetPath(
                    level.OpenTiles,
                    level.ObstructedTiles,
                    Vector3Int.FloorToInt(p),
                    Vector3Int.FloorToInt(game.player.transform.position));

                if (path.Length > 0)
                {
                    for (var i = 0; i < path.Length; i++)
                    {
                        var pi = path[i];
                        DrawRectAt(pi + CenterOffset, 0.5f, Color.magenta);
                        DrawRectAt(pi + CenterOffset, 0.05f, Color.yellow);
                    }

                    DrawRectAt(p, .25f, Color.yellow);

                    var p0 = path[0] + CenterOffset;
                    if (Vector3.Dot(creature.MoveDirection, p - p0) <= 0.0f)
                    {
                        if (path.Length < 2)
                        {
                            creature.MoveDirection = Vector3.zero;
                        }
                        else
                        {
                            p0 = path[1] + CenterOffset;
                            var dir = p0 - p;
                            creature.MoveDirection = dir;
                        }
                    }

                    Debug.DrawLine(p, p0, Color.green);
                    Debug.DrawLine(p, p + creature.MoveDirection, Color.red);
                }
            }
		}

		//if(creature.StateTime > 5f){
		//	creature.StateTime = 0;
		//	animator.SetBool("IsChasing", false);
		//}
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
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
