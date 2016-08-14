using UnityEngine;
using System.Collections;

/**
 * Componente para fazer a câmera seguir o jogador.
 * Necesário acoplar esse script a camera e definir o objeto follow.
 */
public class CameraController : MonoBehaviour {

	public GameObject follow;

	// LateUpdate é chamado após todo processamento da 
	// física ser realizado.
	void LateUpdate () {

		// Posição do objeto seguido.
		Vector3 followPosition = follow.transform.position;

		// Nova posição da câmera.
		Vector3 cameraPosition = new Vector3 (followPosition.x, followPosition.y, transform.position.z);

		// Atualiza a posição da câmera.
		transform.position = cameraPosition;
	}
}