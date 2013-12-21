using UnityEngine;
using System.Collections;

public interface Player{
	 int getVerticalInput();
	
	 int getHorizontalInput();
	
	void collision(Collision other);
}
