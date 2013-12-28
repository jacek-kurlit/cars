using UnityEngine;
using System.Collections.Generic;

public interface KrillSetter{
	List<Krill> initHerd(Transform carPosition, Transform[] krillVis);
	void placeKrills(List<Krill> herd, Transform carPosition);
}
