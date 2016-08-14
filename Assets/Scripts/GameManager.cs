using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 *  Controla o fluxo do jogo (navegação entre fases e menu).
 */
public class GameManager : MonoBehaviour
{

	// Total de fases no jogo.
	const int TOTAL_LEVELS = 3;

	// Fase atual do jogador.
	private int level;

	// Interface de fim de jogo.
	private GameObject endGameMenu = null;

	// Implementa o padrão SINGLETON.
	// Precisamos de apenas uma instância do GameManager acessível
	// em todo o jogo.
	public static GameManager instance = null;
	private int totalPickups = 0;
	private int totalPontos = 0;
	private Text totalPontosText = null;

	int totalLife = 3;

	void Awake ()
	{
		//Check if instance already exists
		if (instance == null) {

			//if not, set instance to this
			instance = this;
		}

		//If instance already exists and it's not this:
		else if (instance != this) {

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);	
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	// Chamado pela unity sempre que uma cena acaba
	// de ser carregada.
	void OnLevelWasLoaded (int level)
	{

		// Cria o menu de fim de jogo.
		CreateEndGameMenu ();

		// Oculta o  menu de fim de jogo.
		endGameMenu.SetActive (false);
	}

	// Cria dinamicamente o menu de fim de jogo.
	// Um Prefab do menu foi previamente criado e
	// encontra-se na pasta Prefabs.
	private void CreateEndGameMenu ()
	{
		// Recupera uma referência ao modelo do prefab.
		GameObject EndMenu = Resources.Load<GameObject> ("Prefabs/EndMenu");

		// Instancia o modelo (adicionando a cena) e guardando uma referência
		// para ele na variável engGameMenu.
		endGameMenu = Instantiate (EndMenu, new Vector3 (1, 1, 1), Quaternion.identity) as GameObject;

		// Recupera referências aos botões do menu associando as respectivas
		// ações de clique.
		Button nextLevelBtn = GameObject.Find ("NextLevelBtn").GetComponent<Button> ();
		nextLevelBtn.onClick.AddListener (() => NextLevel ());

		Button restartLevelBtn = GameObject.Find ("RestartLevelBtn").GetComponent<Button> ();
		restartLevelBtn.onClick.AddListener (() => RestartLevel ());

		Button menuPrincipalBtn = GameObject.Find ("MenuPrincipalBtn").GetComponent<Button> ();
		menuPrincipalBtn.onClick.AddListener (() => MenuPrincipal ());

		this.totalPontosText = GameObject.Find ("pontosText").GetComponent<Text> ();
		totalPontosText.text = "Total de Pontos : " + totalPontos.ToString ();

	}

	// -----------------------------------------totalPickups

	// Inicia um novo jogo.
	public void NewGame ()
	{
		level = 0;
		NextLevel ();
	}

	// Reinicia a fase atual.
	public void RestartLevel ()
	{
		SceneManager.LoadScene ("mapload");
		this.totalPontos -= totalPickups;
	}

	// Avança para a próxima fase.
	public void NextLevel ()
	{
		level++;

		if (level <= TOTAL_LEVELS) {
			SceneManager.LoadScene ("mapload");
		}
	}

	// Encerra a fase atual, exibindo
	// o menu de fim de jogo.
	public void EndLevel (int timeLeft)
	{
		if (level >= TOTAL_LEVELS)
			SceneManager.LoadScene ("menu");
		else if (endGameMenu != null) {
			totalPontos = (50 * (totalPickups)) + ((30 - timeLeft) * 10);
			this.totalPontosText.text = "Total de Pontos : " + totalPontos.ToString ();
			endGameMenu.SetActive (true);
		}
	}

	public void MenuPrincipal ()
	{
		SceneManager.LoadScene ("menu");
		this.totalLife = 3;
	}

	// Sai do jogo.
	public void QuitGame ()
	{
		Application.Quit ();
	}

	public int CurrentLevel ()
	{
		return this.level;
	}

	public bool OnCollision (Collision2D other, int pickupsCounter)
	{

		// Se todos pickups foram coletados encerra a fase.
		// Isso deve ser removido, visto que a fase deverá 
		// ser encerrada quando o UFO for direcionado para o
		// portal.

		if (pickupsCounter >= totalPickups) {
			return true;
		} else {
			return false;
		}
	}

	public void SetTotalPickups (int length)
	{
		this.totalPickups = length;
	}

	public void decrementLife ()
	{
		this.totalLife--;
	}

	public int lifes ()
	{
		return this.totalLife;
	}
}