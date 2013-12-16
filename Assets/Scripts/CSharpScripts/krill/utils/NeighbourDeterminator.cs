using UnityEngine;
using System.Collections.Generic;

public class NeighbourDeterminator {
	
    public List<Krill> determinateNeighbours(List<Krill> herd, Krill krill){
        List<Krill> neighbours = new List<Krill>();
        float senseDistance = calculateSenseDistance(herd,krill);
		
        foreach(Krill other in herd){
			
            if(!(krill==other)){
                float distance = krill.getPosition().distanceFrom(other.getPosition());					
                if(distance <= senseDistance){					
                    neighbours.Add(other);
                }
            }
        }
        return neighbours;
    }

    private float calculateSenseDistance(List<Krill> herd, Krill krill){
        float distance = 0.0f;
        foreach(Krill other in herd){
            distance += krill.getPosition().distanceFrom(other.getPosition());				
        }
		
        return distance/(5.0f * herd.Count);
    }
}
