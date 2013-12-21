using UnityEngine;
using System.Collections.Generic;

public class ForagingCalculator{
 	private  BetaCalculator betaCalculator = new BetaCalculator();
	
    public void calculateForagingMotion(List<Krill> herd, HerdParameters parameters, Food food){
         foreach(Krill krill in herd){
             Position oldForaging = krill.getForagingMotion() * parameters.inertiaForagingWeight;
             Position betaPosition = betaCalculator.calculateBeta(krill,food,parameters);		
			 betaPosition = betaPosition * parameters.Vf;		
		     krill.setForagingMotion(betaPosition + oldForaging);             
         }
    }
}
