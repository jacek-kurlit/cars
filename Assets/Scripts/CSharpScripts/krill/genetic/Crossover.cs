using UnityEngine;
using System.Collections.Generic;

public class Crossover {
    private TendencyCalculator tendencyCalculator = new TendencyCalculator();

    public void crossoverHerd(List<Krill> herd, HerdParameters parameters) {
        foreach (Krill krill in herd) {
            float Cr = 0.2f * tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(), parameters.getBestFitnessValue(), parameters);
            float ran = Random.Range(0.0f,1.0f);
            if (ran < Cr) {
                crossKrill(krill, herd);
            }
        }
    }

    private void crossKrill(Krill toCross, List<Krill> herd) {
        int ind =   Random.Range(0,herd.Count);
       // toCross.crossover(herd[ind]);
    }
}
