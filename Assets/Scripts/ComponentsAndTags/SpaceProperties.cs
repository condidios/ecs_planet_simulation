using Unity.Entities;
using Unity.Mathematics;

namespace PlanetSimulator
{
    public struct SpaceProperties : IComponentData
    {
        public float3 SpaceDimensions;
        public int NumberOfSatellites;
        public Entity SatellitePrefab;
    
    }
}
