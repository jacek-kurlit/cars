using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	
	public Transform[] roadPoints;
	private NavMeshAgent nav;
	private int currentPos = 1;
	private int lastPos;
	// Use this for initialization
	void Start () {
		
		lastPos = roadPoints.Length;
		nav = GetComponent<NavMeshAgent>();
		nav.destination = roadPoints[0].position;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentPos < lastPos){
			if(nav.remainingDistance < nav.stoppingDistance){
				nav.speed = 120f;
				nav.destination = roadPoints[currentPos].position;
				currentPos++;
			}
		}
	}
}
