using UnityEngine;
using System.Collections;

public class LapDetector : MonoBehaviour {
	private float bestTime = 115.0f;
	void OnTriggerEnter(Collider other){
		Timer timer = other.gameObject.transform.parent.gameObject.GetComponent<Timer>();
		if(timer.totalTime < bestTime && timer.enabled){
			bestTime = timer.totalTime;
			FileManager.saveNewRekord("Nowy rekord toru wynosi: " + bestTime);
		}
		if(!timer.enabled)
			timer.enabled = true;	

		timer.totalTime = 0.0f;
	}
}
