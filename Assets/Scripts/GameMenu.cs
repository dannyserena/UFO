using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 *  Responde a interações nos botões do menu inicial.
 *  1) Esse componente precisa ser vinculado a algum game object
 *  2) Os métodos desse component devem ser vinculados aos botões através do 
 *  editor. 
 */
public class GameMenu : MonoBehaviour
{

	// Clique em "Novo Jogo".
	public void NewGame ()
	{
		GameManager.instance.NewGame ();
	}

	// Clique em "Sair"
	public void QuitGame ()
	{
		GameManager.instance.QuitGame ();
	}
}