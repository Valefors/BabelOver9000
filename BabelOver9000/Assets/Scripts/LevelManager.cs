using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[Header("STATES_MUSIC")]
	[SerializeField] string[] statesArray;

	[Header("PATTERNS")]
	[Space()]
	public GameObject[] patternsArray;
	[SerializeField] Transform patternsSpawn;
	//[SerializeField] float spawnPosY = 0f;
	[SerializeField] int NUMBER_LEVELS;
	[Space()]
	[SerializeField] int LEVELS_MAYA;
	[SerializeField] int LEVELS_RENAISSANCE;
	[SerializeField] int LEVELS_KOREA;
    [SerializeField]
    private GameObject clouds;
    private Animator cloudsAnimator;
    [SerializeField] Vector3[] spawnPatternArray;

    public GameObject optionsScreen;


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

	Vector3 firstFinalCamPos = new Vector3(0f, 4f, -16f);
    Vector3 secondFinalCamPos = new Vector3(0f, 55f, -16f);
    //Vector3 firstFinalCamPos = new Vector3(0f, 25f, -45f);

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
        //A REMETTRE
		//NUMBER_LEVELS = patternsArray.Length;

		chantierSprite = Resources.Load<Sprite>("Sprites/Tour/Chantier");

        cloudsAnimator = clouds.GetComponent<Animator>();
	}
	
	public void LaunchLevel(){
		FirstSpawnPattern();
		newFloor = Instantiate(floor, new Vector3(0, 0, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
	}

	void NextLevel(){

		//level++;

		/*if(level == LEVELS_MAYA) print("Maya");
		if(level == LEVELS_RENAISSANCE) print("Renaissance");
		if(level == LEVELS_KOREA) print("Korea");*/

		/*f(level >= NUMBER_LEVELS) {
			UpdateFloorSprite(level-1);
			EndLevel();
			return;
		}*/

		HideBubbles();
        clouds.transform.position = new Vector3(clouds.transform.position.x, floorPosY, -0.29f);
        cloudsAnimator.SetTrigger("CloudAnim");
	}

    public void LaunchTransition()
    {
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
         //AkSoundEngine.PostEvent("Play_Construction", gameObject);

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

         else
        {
            print("can u reload plz");
            StartCoroutine(ReloadScene());
        }
 	}

 	void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
    	if(firstPlay) {
    		firstPlay = false;
    		return;
    	}

    	//if(shouldReplay) return;

        if (in_type == AkCallbackType.AK_MusicSyncEntry)
        {
            level++;

            /*if(level == LEVELS_MAYA) print("Maya");
            if(level == LEVELS_RENAISSANCE) print("Renaissance");
            if(level == LEVELS_KOREA) print("Korea");*/

            if (level == NUMBER_LEVELS)
            {
                //print()
                UpdateFloorSprite(level - 1);
                EndLevel();
                return;
            }
            NextLevel();
            /*if (!shouldReplay){
        		//print("next level");
        		//AddFloor();
				NextLevel();
         	}

         	if (shouldReplay){
         		SpawnPattern();
				MoveBubbles();
				currentBubblesActivate = 0;

				shouldReplay = false;
         	}*/
        }
    }

	void AddFloor()
	{
		//print("AddFloor");
		floorPosY += FLOOR_OFFSET_Y;

		newFloor = Instantiate(floor, new Vector3(0, floorPosY, 0), Quaternion.identity);
		newFloor.GetComponent<SpriteRenderer>().sprite = chantierSprite;
	}

	void UpdateFloorSprite(int pLevel)
	{
		//print("update floor");
		if(pLevel < 15) newFloor.GetComponent<SpriteRenderer>().sprite = spriteFloorsArray[pLevel - 1];
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
		//print(statesArray[level]);

		//AkSoundEngine.PostEvent(dialog.WwiseEvent, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
		float posX = spawnPatternArray[0].x;
        float posY = spawnPatternArray[0].y;
        float posZ = spawnPatternArray[0].z;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, posY, posZ), Quaternion.identity);
    }

	void SpawnPattern()
	{
        //print(statesArray[level]);
		if(level+1 < NUMBER_LEVELS) AkSoundEngine.PostEvent(statesArray[level+1], gameObject); //ATTENTION LENGTH

        float posX = spawnPatternArray[level].x;
        float posY = spawnPatternArray[level].y;
        float posZ = spawnPatternArray[level].z;

        //if (!shouldReplay) posY += FLOOR_OFFSET_Y;

		Instantiate(patternsArray[level].gameObject, new Vector3(posX, posY, posZ), Quaternion.identity);
	}

	void EndLevel(){
		print("END GAME");
		Vector3 camPos = cam.gameObject.transform.position;
        //AkSoundEngine.PostEvent("Play_End", gameObject);
        AkSoundEngine.PostEvent("Play_End", gameObject, (uint)AkCallbackType.AK_MusicSyncEntry, Terminate, null);

        HideBubbles();
		//StartCoroutine(MoveCameraFromTo(cam.gameObject.transform, camPos, firstFinalCamPos, 5));

        //camPos = cam.gameObject.transform.position;
        StartCoroutine(MoveCameraFromTo(cam.gameObject.transform, firstFinalCamPos, secondFinalCamPos, 10));
        
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f);

        //Color color = optionsScreen.GetComponent<Image>().color;
        //color.a = 0f;

        //optionsScreen.GetComponent<Image>().color = color;
        optionsScreen.gameObject.SetActive(true);

        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            // set color with i as alpha
            optionsScreen.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return null;
        }
        //optionsScreen.gameObject.GetComponent<Animator>().SetTrigger("fade");

        yield return new WaitForSeconds(5f);

        for (float i = 2; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            optionsScreen.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return null;
        }

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
