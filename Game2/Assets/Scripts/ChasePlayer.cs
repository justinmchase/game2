using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		var creature = animator.GetComponent<CreatureBehavior>();
		if(creature != null)
        {
            if (creature.StateTime > 5f)
            {
                creature.StateTime = 0;
                animator.SetBool("IsChasing", false);
                creature.SetTarget(null);
                return;
            }

            var game = GameManager.current;
            var target = Vector3Int.FloorToInt(game.player.transform.position);
            var position = Vector3Int.FloorToInt(creature.transform.position);
            if (Vector3Int.Distance(target, position) < 10f)
            {
                creature.SetTarget(target);
            }
            else
            {
                creature.SetTarget(null);
            }
		}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var creature = animator.GetComponent<CreatureBehavior>();
        if (creature != null)
        {
            creature.SetTarget(null);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
