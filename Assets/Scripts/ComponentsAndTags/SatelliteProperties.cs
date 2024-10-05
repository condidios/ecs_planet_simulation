using Unity.Entities;
using Unity.Mathematics;

namespace PlanetSimulator
{
    public struct SatelliteProperties : IComponentData
    {
        public float3 position;
        public float3 velocity;
        public float mass;
    }
}