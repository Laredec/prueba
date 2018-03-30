using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproducirSonidoAnimacion : StateMachineBehaviour
{
    public string[] vNombresSonido;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (animator.gameObject.transform.parent.FindChild(vNombresSonido[Random.Range(0, vNombresSonido.Length)]))
        string nombreSonido = vNombresSonido[Random.Range(0, vNombresSonido.Length)];
        Debug.Log("nombreSonido: " + nombreSonido);
        animator.gameObject.transform.parent.GetComponentInChildren<Atacar>().transform.Find(nombreSonido).GetComponent<ReproducirSonido>().Reproducir();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
