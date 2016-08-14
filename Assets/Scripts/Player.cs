using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 *  Script de controle do UFO.
 */
public class Player : MonoBehaviour
{

	// Velocidade de movimentação do UFO.
	public float speed;

	// Guarda o total de pickups na fase, para verificar
	// se a fase foi concluída. Na versão final do jogo/
	// isso deve ser removido (a fase será concluída quando
	// o jogador acessar o portal).
	private int totalPickups;

	// Conta quantos pickups o jogador pegou.
	private int pickupsCounter;

	// Guarda uma referência ao componente Rigidbody2d do jogador,
	// utilizado para movimentação.
	private Rigidbody2D rb2d;

	// Guarda uma referência ao componente Animator do jogador,
	// usado para animação.
	// Atenção: o animator funcionará apenas na cena "animation",
	// onde ele foi configurado no UFO!
	private Animator animator;

	// Guarda uma referência ao AudioSOurce que emite o som
	// de coleta de pickups.
	//	public AudioSource collectAudio;
	public AudioSource collectAudio;

	// Guarda uma referência ao componente de interface
	// que exibe a quantidade de pickups coletados.
	public Text countText;

	public Text timerText;
	private float timeLeft = 30;

	bool freezerLevel = false;

	// Use this for initialization
	void Start ()
	{
		// Recupera as referências.
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		// Inicia com 0 pickups.
		pickupsCounter = 0;

		// Atualiza o texto de exibição da quantidade de pickups.
		updatePickupsText ();

		for (int f = 3; f > GameManager.instance.lifes (); f--) {
			Image manLife = GameObject.Find ("manLife" + f).GetComponent<Image> ();
			manLife.enabled = false;
		}
	}


	void Update ()
	{
		if (!this.freezerLevel) {
			timeLeft -= Time.deltaTime;
			timerText.text = "Tempo: " + Mathf.RoundToInt (timeLeft).ToString ();
			if (timeLeft < 0) {
				this.freezerLevel = true;
				//			GameOver();
				Image manLife = GameObject.Find ("manLife" + GameManager.instance.lifes ()).GetComponent<Image> ();

				GameManager.instance.decrementLife ();
				manLife.enabled = false;
				if (GameManager.instance.lifes () > 0) {
					GameManager.instance.RestartLevel ();
				} else {
					GameManager.instance.MenuPrincipal ();
				}

			}
		}
				
	}

	// Chamado em intervalos fixos.
	// Uso: atualizacao física.
	void FixedUpdate ()
	{	
		if (rb2d) {

			// Armazena o vetor direção do movimento do UFO.
			Vector2 movement;

			// Diretivas de compilãção.
			#if UNITY_STANDALONE  || UNITY_WEBPLAYER 

			// Compilação PC/Desktop
				
			// Recupera as direções horizontal/vertical
			// dos dispositivos de entrada.
			float vHorizontal = Input.GetAxis ("Horizontal");
			float vVertical = Input.GetAxis ("Vertical");

			movement = new Vector2 (vHorizontal, vVertical);

			// Bônus!
			// Forma alternativa de mover o UFO.
			// Descomente e teste! Use o mouse!
			/*
				if (Input.GetMouseButtonDown (0)) 
				{
					Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					newPosition.z = transform.position.z;
					MoveTo (newPosition);
				}
				*/

			#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE 
				
				// Compilação Mobile

				// Recupera as direções horizontal/vertical do
				// acelerômetro.
				movement = new Vector2(Input.acceleration.x, Input.acceleration.y);

			#endif

			rb2d.AddForce (movement * speed);
		}
	}



	// Chamado quando o UFO colide com algum objeto
	// que possua um Collider setado como isTrigger.
	void OnTriggerEnter2D (Collider2D other)
	{

		// Verifica se o objeto colidido possui a tag
		// "Pickups".
		if (other.gameObject.CompareTag ("Pickups")) {

			animator.SetTrigger ("damage");

			// Remove o pickup
			other.gameObject.SetActive (false);

			// Incrementa o contado de pickups.
			pickupsCounter++;

			// Reproduz o som de pickups.
			collectAudio.volume = 1;
			collectAudio.Play ();

			// Atualiza o texto com a contagem de pickups
			// coletados.
			updatePickupsText ();

		}

	}


	// Chamado quando o UFO colide com algum objeto
	// que possua um Collider não setado como isTrigger.
	void OnCollisionEnter2D (Collision2D other)
	{
		if (GameManager.instance.OnCollision (other, pickupsCounter) && other.gameObject.CompareTag ("Portal")) {
			{

				// Fim de fase.

				// Remove o rb2d do UFO. Assim
				// ele não se moverá mais.
				Destroy (rb2d);

				// Inicia um método em uma coroutine.
				// Uma coroutine pode ser interrompida e 
				// continuada a partir de condições que determinamos.
				StartCoroutine (EndLevel ());
				this.freezerLevel = true;
			}
		}

		Debug.Log (other.gameObject.tag);
		// Dispara a animação de dano.
		// Assim a animação é disparada sempre
		// que o UFO colide com uma parede.

		// Atenção: o animator funcionará apenas na cena "animation",
		// onde ele foi configurado no UFO!
		animator.SetTrigger ("damage");
	}

	// Coroutine de fim de level.
	IEnumerator EndLevel ()
	{
		// Aguarda 1 segundos.
		yield return new WaitForSeconds (1);

		// Exibe o menu.
		GameManager.instance.EndLevel (Mathf.RoundToInt (timeLeft));
	}

	// Atualiza o texto de interface.
	void updatePickupsText ()
	{
		countText.text = "Ouros: " + pickupsCounter;
	}
		
	// Bônus!
	// Forma alternativa de mover o UFO.
	// Descomente e teste! Use o mouse!
	/*
	void MoveTo(Vector3 position)
	{
		StopCoroutine ("MoveToC");
		StartCoroutine ("MoveToC", position);
	}

	IEnumerator MoveToC(Vector3 target)
	{
		while (Vector3.Distance (transform.position, target) > 0.1f) 
		{
			// Lerp gera uma interpolação baseada na distância 
			// e no tempo passado.
			transform.position = Vector3.Lerp (transform.position, target, Time.deltaTime);

			// Ao retornar null a partir de uma Coroutine, ela será 
			// continuada automaticamente no próximo Game Loop!
			yield return null;
		}
	}
	*/
}