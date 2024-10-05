using Unity.Entities;
using Unity.Mathematics;

namespace PlanetSimulator
{
    public struct PlanetProperties : IComponentData
    {
        public float3 position;
        public float mass;
    }
}