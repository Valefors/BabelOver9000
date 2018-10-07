using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[Header("PATTERNS")]
	[Space()]
	public GameObject[] patternsArray;
	[SerializeField] Transform patternsSpawn;
	float spawnPosY = 0;
	[SerializeField] int NUMBER_LEVELS;
	[Space()]
	[SerializeField] int LEVELS_MAYA;
	[SerializeField] int LEVELS_RENAISSANCE;
	[SerializeField] int LEVELS_KOREA;


	[Header("BUBBLES")]
	public int BUBBLES_NUMBER_MAX = 5;
	int currentBubblesActivate = 0;

	public Bubble[] bubblesArray;

	//int patternIndex = 0;

	[Header("FLOORS")]
	public GameObject floor;
	[SerializeField] int floorPosY = 0;
	public int FLOOR_OFFSET_Y = 3;
	[SerializeField] Sprite[] spriteFloorsArray;

	Sprite chantierSprite;
	GameObject newFloor;
	//int spriteIndex = 0;

	Vector3 finalCamPos = new Vector3(0f, 25f, -45f);

	int level = 0;
	public bool shouldReplay = false;

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

		chantierSprite = Resources.Load<Sprite>("Sprites/Chantier");
		newFloor = Instantiate(floor, new Vector3(0, 0, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
	}
	
	public void IncrementBubbleActivateNumber(){
		currentBubblesActivate ++;

		if(currentBubblesActivate == BUBBLES_NUMBER_MAX)
		{
			NextLevel();
		}
	}

	public void CheckReplay() {
		print("zero second");
		if(shouldReplay == true) 
		{
			StartCoroutine(CheckReplayWaitForSeconds());
		}

		//else NextLevel();
	}

	IEnumerator CheckReplayWaitForSeconds()
	{
		yield return new WaitForSeconds(1.0f);

		SpawnPattern();
		MoveBubbles();
		currentBubblesActivate = 0;

		shouldReplay = false;
	}

	void NextLevel(){

		level++;

		if(level == LEVELS_MAYA) print("Maya");
		if(level == LEVELS_RENAISSANCE) print("Renaissance");
		if(level == LEVELS_KOREA) print("Korea");

		if(level >= NUMBER_LEVELS) {
			UpdateFloorSprite(level-1);
			EndLevel();
			return;
		}

		HideBubbles();
		UpdateFloorSprite(level);
		MoveCamera();

		shouldReplay = false;
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

         if(level < NUMBER_LEVELS){
         	MoveBubbles();
         	AddFloor();
         	SpawnPattern();
         }
 	}

	void AddFloor()
	{
		floorPosY += FLOOR_OFFSET_Y;

		newFloor = Instantiate(floor, new Vector3(0, floorPosY, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
		//newFloor.GetComponent<SpriteRenderer>().sprite = spriteFloorsArray[level];
	}

	void UpdateFloorSprite(int pLevel)
	{
		newFloor.GetComponent<SpriteRenderer>().sprite = spriteFloorsArray[pLevel];
	}

	void HideBubbles(){
		for (int i = 0; i < bubblesArray.Length; i++)
		{
			bubblesArray[i].GetComponent<Bubble>().SetModeHide();
		}
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

		if(!shouldReplay) spawnPosY += FLOOR_OFFSET_Y;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, spawnPosY, posZ), Quaternion.identity);
	}

	void EndLevel(){
		print("END GAME");
		Vector3 camPos = Camera.main.gameObject.transform.position;

		HideBubbles();
		StartCoroutine(MoveCameraFromTo(Camera.main.gameObject.transform, camPos, finalCamPos, 10));
	}


}
