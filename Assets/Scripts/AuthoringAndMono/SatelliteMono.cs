using PlanetSimulator;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace AuthoringAndMono
{
    public class SatelliteMono : MonoBehaviour
    {
        public float mass = 1000f;
        public float3 initialVelocity = float3.zero;
    }
    public class SatelliteBaker : Baker<SatelliteMono>
    {
        public override void Bake(SatelliteMono authoring)
        {
            AddComponent(new SatelliteProperties
            {
                mass = authoring.mass,
                velocity = authoring.initialVelocity,
                position = authoring.transform.position 
            });
        }
    }
}