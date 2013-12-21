﻿using UnityEngine;
using System.Collections;

public class EnemyPlayer : Player {
	private Transform[] initialTrace;
	private Transform carTransform;
	private Drivetrain drivetrain;
	private Herd herd;
	private int index = 1;
	
	private int inverse = 1;
	
	private bool back = false;
	private float backTime = 4.0f;
	private float currentBackTime = 0.0f;
	private float angle = 0f;
	
	public EnemyPlayer(GameObject car){
	 	initialTrace =  GameObject.FindGameObjectWithTag("InitialTrace").GetComponentsInChildren<Transform>();
		this.carTransform = car.transform;			
		drivetrain = car.GetComponentInChildren<Drivetrain>();	
		Transform[] krillVis =  GameObject.FindGameObjectWithTag("KrillVisual").GetComponentsInChildren<Transform>(); 
		herd = new Herd();	
		herd.init(carTransform,krillVis,initialTrace);
	}
		
	public int getVerticalInput(){		
		Vector3 bestKrillPosition = herd.simulate();
		return angleDirection(bestKrillPosition);
	}
	
	public int getHorizontalInput(){
		return handleBraking();
	}
	
	public void collision(Collision other){
		if(!other.gameObject.tag.Equals("Player") && currentBackTime == 0.0f){	
			Vector3 heading = initialTrace[index].position - carTransform.position;
			float angle = Vector3.Angle(heading,carTransform.forward);
			if(angle > 30.0f){
				back = true;
			}
		}
	}

	private int angleDirection(Vector3 bestKrillPosition){
		//Debug.Log("Heading to " + bestKrillPosition);
		Vector3 heading = bestKrillPosition - carTransform.position;
		Vector3 perp = Vector3.Cross(carTransform.forward, heading);
		float dir = Vector3.Dot(perp, carTransform.up);
		float angle = Vector3.Angle(heading,carTransform.forward);
		if (dir > 0f) {
			if(angle > 5.0f)
				return 1 * inverse;
		}
		else{
			if(angle > 5.0f)
				return -1 * inverse;				
		}
		return 0;		
	}

	/*private int angleDirection(){
		Vector3 heading = initialTrace[index].position - carTransform.position;
		Vector3 perp = Vector3.Cross(carTransform.forward, heading);
		float dir = Vector3.Dot(perp, carTransform.up);
		float angle = Vector3.Angle(heading,carTransform.forward);
		if (dir > 0f) {
			if(angle > 5.0f)
				return 1 * inverse;
		}
		else if (dir < -0.15f){
			if(angle > 5.0f)
				return -1 * inverse;				
		}
		return 0;		
	}*/
	
	private int handleBraking(){
		/*Vector3 heading = initialTrace[index].position - carTransform.position;
		if(heading.magnitude < 10.0f){
			index = index < initialTrace.Length-1? ++index:1;
			Debug.Log("Change destination to " + index);			
		}*/		
		
		if(drivetrain.getSpeed() < 100){
			return 1;
		}				
		else{						
			return 0;
		}
		
	}	
}