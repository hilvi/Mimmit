using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	
	public GameObject cardPrefab;
	public float ratio;
	public int colCount = 4;
	public int rowCount = 2;
	public float cardDist = 0;
	public Material[] textures; // List of materials for each available card suit
	
	const float cardW = 2.5f; // The default dimensions of a single card, used for calculations
	const float cardH = 2.5f;
	
	const float defaultRowCount = 3f;
	const float defaultColCount = 2f;
	
	Material[] cardTextures;
	
	void Awake() {
		ShuffleMaterials();
		PlaceCards();		
	}
	
	public int CardCount () {
		return (int) colCount*rowCount;
	}
	
	void ShuffleMaterials() {
		AssignMaterials ();
		Material buf;
		
		for (int i=0; i < cardTextures.Length; i++) 
		{ // This "shuffles" the texture array by swapping each element with another one at random
			int newPos = Random.Range (0, cardTextures.Length);
			buf = cardTextures[newPos];
			cardTextures[newPos] = cardTextures[i];
			cardTextures[i] = buf;
		}
	}
	
	void AssignMaterials() 
	{ // This method fills the card texture array with the available suit textures
		int count = CardCount ();
		cardTextures = new Material[count];
		int i;
		Material buf;
		for (i=0; i < textures.Length; i++) 
		{
			int newPos = Random.Range (0, textures.Length - 1);
			buf = textures[newPos];
			textures[newPos] = textures[i];
			textures[i] = buf; 
		}
		
		for (i=0; i < count; i++) 
		{
			cardTextures[i] = textures[(i/2) % textures.Length]; 
			// For three suits, this expression will generate the following repeating sequence of indices: 0,0,1,1,2,2,0,0,1,1,2,2,0,0,1,1,2,2,...
		}
	}
	
	void PlaceCards() {		
		
		GameObject card;
		Transform cardBack;
		Vector3 cardPosition;
		int count = 0;
		
		float i, j;
		
		float shiftW = (colCount - 1f) / 2f; // We calculate these shifts here to reduce the calculations inside the loop
		float shiftH = (rowCount - 1f) / 2f;
		
		if (colCount >= defaultRowCount) 
		{ // We want to resize the cards to fit the screen regardless of the dimensions of the field
			ratio *= defaultRowCount / colCount;
		}
		if (rowCount*ratio >= defaultColCount) 
		{ // The ratio is calculated in relation to the "default" dimensions (2x3)
			ratio *= defaultColCount / (rowCount * ratio);
		}
		
		for (j = -shiftH; j <= shiftH; j++)
			for (i = -shiftW; i <= shiftW; i++) {

				cardPosition =  new Vector3((cardW + cardDist) * i * ratio, (cardH+cardDist)* j * ratio, 0f); // Calculate the card's position
				card = (GameObject)Instantiate(cardPrefab, cardPosition, Quaternion.identity); // Place the card prefab onto the stage

			
				cardBack = card.transform.Find("Face"); 
				cardBack.renderer.material = new Material(Shader.Find("Diffuse"));
				cardBack.renderer.material = cardTextures[count]; // Apply the appropriate material to the card's face
				
				cardBack.transform.localScale *= ratio; // Resize the card according to the calculated ratio
				card.transform.Find ("Back").localScale *= ratio;
				
				card.GetComponent<Card>().SetSuit(cardTextures[count].name); // Save the card's suit in the card object (used to determine the card's suit later)
			
				count ++;
			}
	}
}
