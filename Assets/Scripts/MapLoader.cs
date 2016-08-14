using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

/**
 * Cria um level dinamicamente a partir de um arquivo de texto.
 * O arquivo representa uma matriz onde cada elemento representa
 * um tile que irá compor o cenário.
 * 
 * Esse componente está incompleto e foi usado em sala apenas
 * para demonstração de conceitos. Os métodos presentes nele 
 * devem ser migrados para o GameManager que é responsável
 * pela navegação entre as fases.
 * 
 * O funcionamento deste codigo pode ser visto na cena "mapload".
 * 
 * DICA: você pode criar um game object vazio na cena para ser 
 * o CONTAINER de todos os tiles que você criar do mapa.
 * Assim, ao encerrar a fase, você só precisa remover esse CONTAINER
 * da cena. Use "Create Empty" e tile.transform.parent = container.transform;
 */
public class MapLoader : MonoBehaviour
{

	void Start ()
	{
		CreateLevel (GameManager.instance.CurrentLevel ());
		// Identifica quantos pickups a fase tem.
		GameManager.instance.SetTotalPickups (GameObject.FindGameObjectsWithTag ("Pickups").Length);

	}

	// Cria os tiles do level a partir
	// da matriz carregada do arquivo de texto.
	void CreateLevel (int level)
	{
		string[][] data = LoadLevelData (level);

		for (int y = 0; y < data.Length; y++) {
			for (int x = 0; x < data [0].Length; x++) {

				switch (data [y] [x]) {
				case "1":
					CreateTile ("Wall" + Random.Range (0, 3), x, y);
					break;
				case "0":
					CreateTile ("Floor" + Random.Range (0, 3), x, y);
					break;
				case "+":
					CreateTile ("Portal", x, y);
					break;
				case "-":
					CreateTile ("FloorPortal", x, y);
					break;
				case "x":
					CreateTile ("Trap", x, y);
					break;
				case "@":
					CreateTile ("Floor" + Random.Range (0, 3), x, y);
					CreateTile ("Pickup", x, y);
					break;
				}
			}
		}


	}

	// Cria um tile de um dado tipo em uma dada
	// posição.
	// Sugere-se altear o parâmetro "string prefab"
	// para um array de prefabs, e selecionar um
	// prefab aleatório dentro desse array.
	// Assim a fase fica mais diversificada.
	GameObject CreateTile (string prefab, int x, int y)
	{
		GameObject Prefab = Resources.Load<GameObject> ("Prefabs/" + prefab);
		Instantiate (Prefab, new Vector3 (x - 4, y, 0), Quaternion.identity);
		return Prefab;
	}

	// Retorna a matriz que representa o level informado.
	// Por padrão os arquivos de level devem se chamar
	// level1, level2, etc.
	string[][] LoadLevelData (int level)
	{
		// Caminho do arquivo dentro da pasta "Resources".
		string path = "Maps/level" + level;

		// Carrega o arquivo.
		TextAsset textFile = (TextAsset)Resources.Load (path);

		// Recupera o texto do arquivo.
		string text = textFile.text;

		// Realiza splits transformando o texto em
		// uma matriz.
		string[] lines = Regex.Split (text, "\n");
		int rows = lines.Length;
		string[][] data = new string[rows][];
		for (int i = 0; i < rows; i++) {
			string[] lineParts = Regex.Split (lines [i], " ");
			data [(rows - 1) - i] = lineParts;
		}

		return data;
	}


		
}
