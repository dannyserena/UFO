using UnityEngine;
using System.Collections;

/**
 * Gira um objeto na velocidade informada.
 * Util para pickups do jogo!
 */
public class Rotator : MonoBehaviour {

	public float speed = 45;
	
	// Update is called once per frame
	void Update () 
	{
		// Aplica a rotação apenas no eixo Z.
		// Usa o tempo como referência pois desejamos
		// que a rotação possua a velocidade de 
		// speed graus por segundo!
		transform.Rotate (new Vector3(0,0,speed)*Time.deltaTime);
	}
}
