using PlanetSimulator;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace AuthoringAndMono
{
    public class SpaceMono : MonoBehaviour
    {
        public float3 SpaceDimensions;
        public int NumberOfSatellites;
        public GameObject SatellitePrefab;
        public uint RandomSeed;
    }

    public class SpaceBaker : Baker<SpaceMono>
    {
        public override void Bake(SpaceMono authoring)
        {
            AddComponent(new SpaceProperties
            {
                SpaceDimensions = authoring.SpaceDimensions,
                NumberOfSatellites = authoring.NumberOfSatellites,
                SatellitePrefab = GetEntity(authoring.SatellitePrefab)
            });
            AddComponent(new SpaceRandom
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed)
            });
        }
    }
}