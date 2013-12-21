using UnityEngine;
using System.Collections.Generic;

public class MotionCalculator {
 	private AlphaCalculator alphaCalculator = new AlphaCalculator();

    public void calculateMotion(List<Krill> herd, HerdParameters algorithmParameters){
        foreach(Krill krill in herd){
            Position oldMotion = krill.getMotionInduced()*algorithmParameters.inertiaWeight;
            Position alphaPosition = getAlphaPosition(herd,krill,algorithmParameters);
            Position newMotionPosition = alphaPosition + oldMotion;

            krill.setMotionInduced(newMotionPosition);
        }
    }

    private Position getAlphaPosition(List<Krill> herd,Krill krill, HerdParameters parameters){
        Position alphaPosition = alphaCalculator.calculateAlpha(herd,krill,parameters);
        return alphaPosition * parameters.N_MAX;
    }
}
