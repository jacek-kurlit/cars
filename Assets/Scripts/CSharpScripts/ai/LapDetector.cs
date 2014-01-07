using UnityEngine;
using System.Collections.Generic;

public class LapDetector : MonoBehaviour {
	private float bestSectorTime;
	public int id;
	private SectorsManager sectorManger;

	void Start(){
		sectorManger =  GameObject.FindGameObjectWithTag("Sector").GetComponent<SectorsManager>();
		bestSectorTime = sectorManger.getBestSectorTime(id);	
	}

	void OnTriggerEnter(Collider other){
		Timer timer = other.gameObject.transform.parent.gameObject.GetComponent<Timer>();
		GameObject carGameObject = other.transform.parent.gameObject;
		if(carGameObject.tag=="Enemy"){
			newSectorRecord(timer,carGameObject);
		}

		timer.sectorTime = 0.0f;
	}

	private void newSectorRecord(Timer timer,GameObject gameObject){
		Player player =  gameObject.GetComponent<CarController>().player;
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		if(timer.sectorTime < bestSectorTime){
			Debug.Log("Nowy rekord sektora " + id);
			bestSectorTime = timer.sectorTime;

			player.setGeneralChange(true);
			List<Vector3> points = replaceVectors(gameObject);
			sectorManger.saveNewSectorVectors(id,bestSectorTime,points,pointsAnalizer.lap);
		}else{
			if(player.isNewTrace(id)){
				player.setSectorVectors(sectorManger.getSectorVectors(id),id);
				pointsAnalizer.resetPoints();
				player.tryNewTrace(false,id);
			}else if(!player.isGeneralChange()){
				replaceVectors(gameObject);
				player.tryNewTrace(true,id);
			}else{
				pointsAnalizer.resetPoints();
			}

		}

	}

	private List<Vector3> replaceVectors(GameObject gameObject){
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		Player player =  gameObject.GetComponent<CarController>().player;
		
		List<Vector3> points = player.getSectorVectors(id); 
		player.setSectorVectors(pointsAnalizer.getNewSectorPoints(),id);
		pointsAnalizer.resetPoints();

		return points;
	}

}
