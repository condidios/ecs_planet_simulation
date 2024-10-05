using Unity.Entities;
using Unity.Mathematics;

namespace PlanetSimulator
{
    public struct SpaceRandom : IComponentData
    {
        public Random Value;
    }
}