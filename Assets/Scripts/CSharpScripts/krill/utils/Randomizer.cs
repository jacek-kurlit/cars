using UnityEngine;
using System.Collections.Generic;

public class Randomizer {
    public List<Krill> randomizeHerd(HerdParameters algorithmParameters,Vector3 carPosition,
		Transform[] krillVis ){
        List<Krill> herd = new List<Krill>(algorithmParameters.herdSize);

        for (int i = 1; i < krillVis.Length; i++) {			
            herd.Add(randomKrill(carPosition,krillVis[i]));
        }
		
        return herd;
    }

    private Krill randomKrill(Vector3 carPosition, Transform krillVis){
        float poz_x = Random.Range(carPosition.x,carPosition.x + 10.0f);			
		float poz_z = Random.Range(carPosition.z,carPosition.z - 10.0f);			
		
        Position position = new Position(poz_x,poz_z);
		krillVis.position = new Vector3(poz_x,carPosition.y + 2.0f,poz_z);

        return new Krill(position, krillVis);
    }
}
