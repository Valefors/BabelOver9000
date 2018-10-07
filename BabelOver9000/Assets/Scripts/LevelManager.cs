using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[Header("PATTERNS")]
	public GameObject[] patternsArray;
	[SerializeField] Transform patternsSpawn;
	float spawnPosY = 0;
	[SerializeField] int[] numberLevelsArray;

	[Header("BUBBLES")]
	public int BUBBLES_NUMBER_MAX = 5;
	int currentBubblesActivate = 0;

	public Bubble[] bubblesArray;

	//int patternIndex = 0;
	int NUMBER_LEVELS;

	[Header("FLOORS")]
	public GameObject floor;
	[SerializeField] int floorPosY = 0;
	public int FLOOR_OFFSET_Y = 3;
	[SerializeField] Sprite[] spriteFloorsArray;
	//int spriteIndex = 0;

	int level = 0;

	//		float newY = GetComponent<Transform>().position.y + 2;
	//	GetComponent<Transform>().position = new Vector3(0, newY, 0);

	 #region SINGLETON PATTERN
 	public static LevelManager _instance;
 	public static LevelManager instance
 	{
     get {
         if (_instance == null)
         {
             _instance = GameObject.FindObjectOfType<LevelManager>();
         }
     
         return _instance;
     	}
 	}
 	#endregion

	// Use this for initialization
	void Start () {
		FirstSpawnPattern();
		NUMBER_LEVELS = patternsArray.Length;
	}
	
	public void IncrementBubbleActivateNumber(){
		currentBubblesActivate ++;

		if(currentBubblesActivate == BUBBLES_NUMBER_MAX)
		{
			NextLevel();
		}
	}

	void NextLevel(){

		level++;

		if(level >= NUMBER_LEVELS) {
			print("END GAME");
			return;
		}

		MoveCamera();
	}

	void MoveCamera()
	{
		Vector3 camPos = Camera.main.gameObject.transform.position;

		float newX = camPos.x;
		float newY = camPos.y + FLOOR_OFFSET_Y;
		float newZ = camPos.z;

		Vector3 newPos = new Vector3(newX, newY, newZ);

		StartCoroutine(MoveCameraFromTo(Camera.main.gameObject.transform, camPos, newPos, 5));
	}

	IEnumerator MoveCameraFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed) 
	{
         float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
         float t = 0;

         while (t <= 1.0f) 
         {
             t += step;
             objectToMove.position = Vector3.Lerp(a, b, t);
             yield return new WaitForFixedUpdate();
         }
         objectToMove.position = b;

         MoveBubbles();
         AddFloor();
         SpawnPattern();
 	}

	void AddFloor()
	{
		floorPosY += FLOOR_OFFSET_Y;

		GameObject newFloor = Instantiate(floor, new Vector3(0, floorPosY, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = spriteFloorsArray[level];
	}

	void MoveBubbles()
	{
		currentBubblesActivate = 0;

		for (int i = 0; i < bubblesArray.Length; i++)
		{
			bubblesArray[i].GetComponent<Bubble>().SetModeNormal();
		}

	}

	void FirstSpawnPattern(){
		float posX = patternsSpawn.position.x;
		float posZ = patternsSpawn.position.z;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, spawnPosY, posZ), Quaternion.identity);
	}

	void SpawnPattern(){
		float posX = patternsSpawn.position.x;
		float posZ = patternsSpawn.position.z;

		spawnPosY += FLOOR_OFFSET_Y;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, spawnPosY, posZ), Quaternion.identity);
	}


}
