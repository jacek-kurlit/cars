using UnityEngine;
using System.Collections;

public class EnemyPlayer : Player {
	private Transform[] initialTrace;
	private Transform carTransform;
	private Drivetrain drivetrain;
	private Herd herd;
	private int index = 1;
	
	private int inverse = 1;
	
	private bool newCollsion = true;
	private float stopTime = 3.0f;
	private float currentStopTime = 0.0f;

	private Krill bestKrill;
	
	public EnemyPlayer(GameObject car,Transform visualFoodTransofrm,Transform[] krillVis){
	 	initialTrace =  GameObject.FindGameObjectWithTag("InitialTrace").GetComponentsInChildren<Transform>();
		this.carTransform = car.transform;			
		drivetrain = car.GetComponentInChildren<Drivetrain>();	
		herd = new Herd(carTransform,krillVis,initialTrace,visualFoodTransofrm);	
	}
		
	public int getVerticalInput(){		
		bestKrill = herd.simulate();
		return angleDirection(bestKrill.getPosition().toVector3());
	}
	
	public int getHorizontalInput(){
		return handleBraking();
	}

	private int angleDirection(Vector3 bestKrillPosition){
		Vector3 heading = bestKrillPosition - carTransform.position;
		Vector3 perp = Vector3.Cross(carTransform.forward, heading);
		float dir = Vector3.Dot(perp, carTransform.up);
		float angle = Vector3.Angle(heading,carTransform.forward);
		if (dir > 0f) {
			if(angle > 2.0f)
				return 1;
		}
		else{
			if(angle > 2.0f)
				return -1;				
		}
		return 0;		
	}

	private int handleBraking(){		
		if(drivetrain.getSpeed() < bestKrill.getHighSpeed()){
			return 1;
		}else if(drivetrain.getSpeed() >=  bestKrill.getLowSpeed() && drivetrain.getSpeed() <= bestKrill.getHighSpeed()){
			return 0;
		}									
		return -1;				
	}	

	public void resetPosition(){
		Vector3 foodVector = herd.getCurrentFoodPosition();
		Quaternion rotation =  carTransform.rotation;
		float yRotation = rotation.eulerAngles.y;
		rotation.eulerAngles =  new Vector3(0.0f,yRotation,0.0f);
		carTransform.rotation = rotation;
		carTransform.position = foodVector;
	}	
}
