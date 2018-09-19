using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

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

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		var game = GameObject.Find("map").GetComponent<GameManager>();
		var creature = animator.GetComponent<CreatureBehavior>();
		if(creature != null){
			var dir = (game.player.transform.position - creature.transform.position);
			dir.z = 0;
			//dir.Normalize();
			creature.MoveDirection = dir.normalized;
		}

		if(creature.StateTime > 5f){
			creature.StateTime = 0;
			animator.SetBool("IsChasing", false);
		}
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
