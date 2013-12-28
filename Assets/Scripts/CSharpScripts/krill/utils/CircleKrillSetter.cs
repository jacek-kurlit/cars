﻿using UnityEngine;
using System.Collections.Generic;

public class CircleKrillSetter : KrillSetter {
	
	private HerdParameters parameters;
	
	private float angle = 20.0f;
	private float height = 5.0f;
	
	public CircleKrillSetter(HerdParameters parameters){
		this.parameters = parameters;
	}
	
	public List<Krill> initHerd(Transform carPosition, Transform[] krillVis){
		List<Krill> herd = new List<Krill>();
		float currentAngle = carPosition.rotation.eulerAngles.y - (parameters.herdSize - parameters.herdSize%2)/2.0f * angle; 
		
		
		for(int i = 1; i <= parameters.herdSize; i++){
			Debug.Log("Current Angle " + currentAngle);
			Position position = calculateNewPosition(carPosition,currentAngle);
			currentAngle += angle;
			herd.Add(new Krill(position,krillVis[i]));
		}
		
		return herd;
	}
	
	public void placeKrills(List<Krill> herd, Transform carPosition){
		float currentAngle = 0.0f;
		
		foreach(Krill krill in herd){
			Position position = calculateNewPosition(carPosition,currentAngle);
			currentAngle += angle;
			krill.updatePosition(carPosition.position,position);
		}
	}
	
	private Position calculateNewPosition(Transform carPosition, float currentAnglel){
		Vector3 faceRotation = carPosition.eulerAngles;
		faceRotation.y = currentAnglel;
		Quaternion newRotation = new Quaternion();
		newRotation.eulerAngles = faceRotation;
		Vector3 newPosition = carPosition.position + height * (newRotation * Vector3.right);
		
		return new Position(newPosition);
	}
}