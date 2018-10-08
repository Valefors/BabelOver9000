using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[Header("STATES_MUSIC")]
	[SerializeField] string[] statesArray;

	[Header("PATTERNS")]
	[Space()]
	public GameObject[] patternsArray;
	[SerializeField] Transform patternsSpawn;
	[SerializeField] float spawnPosY = 0f;
	[SerializeField] int NUMBER_LEVELS;
	[Space()]
	[SerializeField] int LEVELS_MAYA;
	[SerializeField] int LEVELS_RENAISSANCE;
	[SerializeField] int LEVELS_KOREA;


	[Header("BUBBLES")]
	public int BUBBLES_NUMBER_MAX = 5;
	int currentBubblesActivate = 0;

	public Bubble[] bubblesArray;
	[SerializeField] Camera cam;

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
	bool firstPlay = true;

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
		NUMBER_LEVELS = patternsArray.Length;

		chantierSprite = Resources.Load<Sprite>("Sprites/Tour/Chantier");
	}
	
	public void LaunchLevel(){
		FirstSpawnPattern();
		newFloor = Instantiate(floor, new Vector3(0, 0, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
	}

	/*public void IncrementBubbleActivateNumber(){
		currentBubblesActivate ++;

		if(currentBubblesActivate == BUBBLES_NUMBER_MAX)
		{
			NextLevel();
		}
	}*/

	/*public void CheckReplay() {
		print("zero second");
		if(shouldReplay == true) 
		{
			StartCoroutine(CheckReplayWaitForSeconds());
		}

		//else NextLevel();
	}*/

	/*IEnumerator CheckReplayWaitForSeconds()
	{
		yield return new WaitForSeconds(1.0f);

		SpawnPattern();
		MoveBubbles();
		currentBubblesActivate = 0;
	}*/

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
		Vector3 camPos = cam.gameObject.transform.position;

		float newX = camPos.x;
		float newY = camPos.y + FLOOR_OFFSET_Y;
		float newZ = camPos.z;

		Vector3 newPos = new Vector3(newX, newY, newZ);

		StartCoroutine(MoveCameraFromTo(cam.gameObject.transform, camPos, newPos, 5));
	}

	IEnumerator MoveCameraFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed) 
	{
         float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
         float t = 0;
         AkSoundEngine.PostEvent("Play_Construction", gameObject);

         while (t <= 1.0f) 
         {
             t += step;
             objectToMove.position = Vector3.Lerp(a, b, t);
             yield return new WaitForFixedUpdate();
         }
         objectToMove.position = b;

         if(level < NUMBER_LEVELS){
         	SpawnPattern();
			MoveBubbles();
			AddFloor();
         }
 	}

 	void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
    	if(firstPlay) {
    		firstPlay = false;
    		return;
    	}

    	print(shouldReplay);
    	//if(shouldReplay) return;

        if (in_type == AkCallbackType.AK_MusicSyncEntry)
        {
        	if(!shouldReplay){
        		print("next level");
        		//AddFloor();
				NextLevel();
         	}

         	if (shouldReplay){
         		SpawnPattern();
				MoveBubbles();
				currentBubblesActivate = 0;

				shouldReplay = false;
         	}
        }
    }

	void AddFloor()
	{
		print("AddFloor");
		floorPosY += FLOOR_OFFSET_Y;

		newFloor = Instantiate(floor, new Vector3(0, floorPosY, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
	}

	void UpdateFloorSprite(int pLevel)
	{
		print("update floor");
		newFloor.GetComponent<SpriteRenderer>().sprite = spriteFloorsArray[pLevel];
		AkSoundEngine.PostEvent("End_Construction", gameObject);
		
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
		//AkSoundEngine.PostEvent("Stop_Menu_Amb", gameObject);
		AkSoundEngine.PostEvent(statesArray[0], gameObject);
		AkSoundEngine.PostEvent("Play_Music", gameObject, (uint)AkCallbackType.AK_MusicSyncEntry, Terminate, null);
		//AkSoundEngine.SetState("Stage", statesArray[level + 1]);
		print(statesArray[level]);

		//AkSoundEngine.PostEvent(dialog.WwiseEvent, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
		float posX = patternsSpawn.position.x;
		print(patternsSpawn.position.x);
		float posZ = patternsSpawn.position.z;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, spawnPosY, posZ), Quaternion.identity);
	}

	void SpawnPattern()
	{
		print("SpawnPattern");
		AkSoundEngine.PostEvent(statesArray[level + 1], gameObject); //ATTENTION LENGTH

		float posX = patternsSpawn.position.x;
		float posZ = patternsSpawn.position.z;

		if(!shouldReplay) spawnPosY += FLOOR_OFFSET_Y;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, spawnPosY, posZ), Quaternion.identity);
	}

	void EndLevel(){
		print("END GAME");
		Vector3 camPos = cam.gameObject.transform.position;

		HideBubbles();
		StartCoroutine(MoveCameraFromTo(cam.gameObject.transform, camPos, finalCamPos, 10));
	}


}
