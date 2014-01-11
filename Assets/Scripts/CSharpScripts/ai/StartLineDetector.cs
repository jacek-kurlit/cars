using UnityEngine;
using System.Collections.Generic;

public class StartLineDetector : MonoBehaviour {

	public int id;
	private float bestLapTime;
	private float bestSectorTime;
	private SectorsManager sectorManger;


	void Start(){
		sectorManger =  GameObject.FindGameObjectWithTag("Sector").GetComponent<SectorsManager>();
		bestLapTime = sectorManger.getBestLapTime();
		bestSectorTime = sectorManger.getBestSectorTime(id);
	}

	void OnTriggerEnter(Collider other){
		Timer timer = other.gameObject.transform.parent.gameObject.GetComponent<Timer>();
		GameObject carGameObject = other.transform.parent.gameObject;
		if(carGameObject.tag == "Enemy" && timer.enabled){
			newSectorRecord(timer,carGameObject);
			newLapRecord(timer,carGameObject);
		}

		if(!timer.enabled)
			timer.enabled = true;	
		timer.sectorTime = 0.0f;
		timer.totalTime = 0.0f;
	}

	private void newLapRecord(Timer timer, GameObject gameObject){
		Player player =  gameObject.GetComponent<CarController>().player;
		if(timer.totalTime < bestLapTime && timer.enabled){
			PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
			bestLapTime = timer.totalTime;
			FileManager.saveNewRekord(bestLapTime,pointsAnalizer.lap);
		}
		saveLap(gameObject,timer);
		resetPoints(gameObject);
	}

	protected void newSectorRecord(Timer timer,GameObject gameObject){
		Player player =  gameObject.GetComponent<CarController>().player;
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		if(timer.sectorTime < bestSectorTime){
			bestSectorTime = timer.sectorTime;

			player.setGeneralChange(true);
			List<Vector3> points = replaceVectors(gameObject);
			sectorManger.saveNewSectorVectors(id,bestSectorTime,points,pointsAnalizer.lap);
		}else{		
			if(player.isNewTrace(id)){
				player.setSectorVectors(sectorManger.getSectorVectors(id),id);
				pointsAnalizer.resetPoints();
				player.tryNewTrace(false,id);
			}else if (!player.isGeneralChange()){
				replaceVectors(gameObject);
				player.tryNewTrace(true,id);
			}else{
				pointsAnalizer.resetPoints();
			}
			
		}
	}
	
	protected List<Vector3> replaceVectors(GameObject gameObject){
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		Player player =  gameObject.GetComponent<CarController>().player;
		List<Vector3> points = player.getSectorVectors(id); 
		player.setSectorVectors(pointsAnalizer.getNewSectorPoints(),id);
		pointsAnalizer.resetPoints();
		
		return points;
	}

	protected void resetPoints(GameObject gameObject){
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		Player player =  gameObject.GetComponent<CarController>().player;
		pointsAnalizer.setAllMapPoints(player.getAllMapPoints());
		player.setGeneralChange(false);

	}

	protected void saveLap(GameObject gameObject,Timer timer){
		PointsAnalizer pointsAnalizer = gameObject.GetComponent<PointsAnalizer>();
		CarController carController = gameObject.GetComponent<CarController>();
		Player player =  gameObject.GetComponent<CarController>().player;
		
		FileManager.saveLap(carController.carId,pointsAnalizer.lap,timer.totalTime);
		pointsAnalizer.lap = pointsAnalizer.lap + 1;
	}
}
