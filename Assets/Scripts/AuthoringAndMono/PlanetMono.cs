using PlanetSimulator;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
    public class PlanetMono : MonoBehaviour
    {
        public float mass = 5.972e+24f; 
    }

    public class PlanetBaker : Baker<PlanetMono>
    {
        public override void Bake(PlanetMono authoring)
        {
            AddComponent(new PlanetProperties
            {
                mass = authoring.mass,
                position = authoring.transform.position 
            });
        }
    }
}