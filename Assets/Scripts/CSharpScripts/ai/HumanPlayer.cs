using UnityEngine;
using System.Collections.Generic;

public class HumanPlayer : Player {
	public int getVerticalInput(){
		if (Input.GetKey (KeyCode.LeftArrow))
			return -1;
		if (Input.GetKey (KeyCode.RightArrow))
			return 1;
		return 0;
	}
	
	public int getHorizontalInput(){
		if(Input.GetKey(KeyCode.UpArrow))
			return 1;
		if(Input.GetKey(KeyCode.DownArrow))
			return -1;
		return 0;
	}

	public void resetPosition(){

	}
	public List<Vector3> getSectorVectors(int sectorId){
		return null;
	}

	public void setSectorVectors(List<Vector3> vectors,int sectorId){
	}
	public List<Vector3> getAllMapPoints(){
		return null;
	}

	public bool isNewTrace(int sectorID){
		return false;
	}
	public void tryNewTrace(bool newTrace,int sectorID){

	}

	public bool isGeneralChange(){
		return false;
	}
	public void setGeneralChange(bool change){
	}
}
