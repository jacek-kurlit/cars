using UnityEngine;
using System.Collections;

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
}
