using UnityEngine;
using System.Collections.Generic;

public class Mutation {
    private TendencyCalculator tendencyCalculator = new TendencyCalculator();

    public void mutateHerd(List<Krill> herd, HerdParameters parameters) {
        foreach (Krill krill in herd) {
            float Mu = 0.05f / tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(), parameters.getBestFitnessValue(), parameters);
            float ran = Random.Range(0.0f,1.0f);

            if (ran < Mu) {
                mutateKrill(krill, herd, parameters);
            }
        }
    }

    private void mutateKrill(Krill krill, List<Krill> herd, HerdParameters parameters) {
        int range = herd.Count / 2;
        Krill first = herd[Random.Range(0,range)];
        int secondIndex = Random.Range(range,herd.Count);
        Krill second = herd[secondIndex];
        krill.mutate(first,second,parameters.getBestFitnessKrill());
    }
}
