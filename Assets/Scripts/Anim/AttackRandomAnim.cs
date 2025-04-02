using UnityEngine;

public class AttackRandomAnim : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetFloat("AttackInt",Random.Range(0,4));
    }

}
