using UnityEngine;
using System.Collections.Generic;

public class DiffusionCalculator{

 public void calculateDiffusionMotion(List<Krill> herd,HerdParameters parameters){
        foreach(Krill krill in herd){
            Position randomizedPosition = randomizePosition();
			randomizedPosition = randomizedPosition * parameters.randomizeDMAX();
            randomizedPosition = randomizedPosition * calculateIterationRatio(parameters);            

            krill.setDiffusionMotion(randomizedPosition);
        }

    }

    private Position randomizePosition(){
        //ranges [-1,1]
        float x = Random.Range(-1.0f,1.0f);
		float z = Random.Range(-1.0f,1.0f);
		
        return new Position(x,z);
    }

    private float calculateIterationRatio(HerdParameters parameters){
        return 1.0f - parameters.getIterationRatio();    
    }
}
