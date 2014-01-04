using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject[] players;
	public GUIText playerIDText;
	public GUIText playerSpeed;
	public GUIText playerTime;

	private int currentIndex = -1;
	private CarCamera carCamera;


	private Drivetrain drivetrain;
	private Timer timer;

	void Start () {
		carCamera = gameObject.GetComponent<CarCamera>();
		switchCamera();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)){
			switchCamera();
		}
		updateHud();
	}

	private void switchCamera(){
		incrementIndex();
		carCamera.target = players[currentIndex].transform;
		string carId = players[currentIndex].GetComponent<CarController>().carId;
		playerIDText.text = carId;
		drivetrain = players[currentIndex].GetComponent<Drivetrain>();
		timer = players[currentIndex].GetComponent<Timer>();
	}

	private void incrementIndex(){
		if(currentIndex + 1 < players.Length)
			currentIndex++;
		else
			currentIndex = 0;
	}

	private void updateHud(){
		playerSpeed.text = "Speed: " + drivetrain.carSpeed;
		playerTime.text = "Time: " + timer.totalTime;
	}
}
