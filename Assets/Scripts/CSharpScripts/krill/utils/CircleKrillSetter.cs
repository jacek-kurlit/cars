﻿using UnityEngine;
using System.Collections.Generic;

public class CircleKrillSetter : KrillSetter {

	private HerdParameters parameters;

	private float angle;
	private float height = 5.0f;
	private const float triangleAngleSize = 60.0f;
	private float delta;
	private float triangleArm;
	private int partSize;
	private int herdSize;
	private float[,] speeds;

	public CircleKrillSetter(HerdParameters parameters){
		this.parameters = parameters;
		speeds = new float[5,2]{ {100.0f,120.0f} , {120.0f,160.0f}, {170.0f,200.0f},{120.0f,160.0f},{100.0f,120.0f}};
		//speeds = new float[3,2]{{120.0f,160.0f}, {170.0f,200.0f},{120.0f,160.0f}};
		//speeds = new float[7,2]{ {80.0f,100.0f} , {100.0f,120.0f}, {120.0f,160.0f},{160.0f,200.0f},{120.0f,160.0f},{100.0f,120.0f},{80.0f,100.0f}};
		partSize = (parameters.herdSize - parameters.herdSize%2)/2;
		angle = triangleAngleSize/(parameters.herdSize - parameters.herdSize%2);
		triangleArm = 2.0f * height * Mathf.Sqrt(3.0f)/3.0f;
		delta = (triangleArm - height)/partSize;
		herdSize = parameters.herdSize;
	}

	public List<Krill> initHerd(Transform carPosition, Transform[] krillVis){
		List<Krill> herd = new List<Krill>();
		float currentAngle = carPosition.rotation.eulerAngles.y - partSize * angle; 
		float currentHeight = triangleArm;

		for(int i = 0; i < herdSize; i++){		
			Position position = calculateNewPosition(carPosition,currentAngle,currentHeight);		
			currentHeight = calculateCurrentHeight(currentHeight,i);
			currentAngle += angle;					
			herd.Add(new Krill(position,krillVis[i],speeds[i,0],speeds[i,1]));
		}

		return herd;
	}
	
	public void placeKrills(List<Krill> herd, Transform carPosition){
		float currentAngle = carPosition.rotation.eulerAngles.y - (herdSize - herdSize%2)/2.0f * angle; 
		float currentHeight = triangleArm;
		int i = 0;

		foreach(Krill krill in herd){
			Position position = calculateNewPosition(carPosition,currentAngle,currentHeight);
			
			currentHeight = calculateCurrentHeight(currentHeight,i);
			currentAngle += angle;
//		    krill.updatePosition(carPosition.position,position);
			i++;
		}
	}

	private Position calculateNewPosition(Transform carPosition, float currentAnglel, float currentHeight){
		Vector3 faceRotation = carPosition.eulerAngles;
		faceRotation.y = currentAnglel;
		Quaternion newRotation = new Quaternion();
		newRotation.eulerAngles = faceRotation;
		Vector3 newPosition = carPosition.position + height * (newRotation * Vector3.forward);

		return new Position(newPosition);
	}

	private float calculateCurrentHeight(float currentHeight, int index){
		if(index <= partSize)
			return currentHeight - delta;
		else
			return currentHeight + delta;

	}			                                                        
}