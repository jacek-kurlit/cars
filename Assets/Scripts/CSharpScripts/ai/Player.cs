using UnityEngine;
using System.Collections.Generic;

public interface Player{
	 int getVerticalInput();
	
	 int getHorizontalInput();

	 void resetPosition();

	List<Vector3> getSectorVectors(int sectorId);

	void setSectorVectors(List<Vector3> vectors,int sectorId);
	List<Vector3> getAllMapPoints();

	bool isNewTrace(int sectorID);
	void tryNewTrace(bool newTrace,int sectorID);

	bool isGeneralChange();
	void setGeneralChange(bool change);

}
