using PlanetSimulator;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(GravitationalForceSystem))] 
    public partial struct UpdateSatelliteTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (satellite, localTransform) in SystemAPI.Query<SatelliteProperties, RefRW<LocalTransform>>())
            {
                localTransform.ValueRW.Position = satellite.position;
                
                localTransform.ValueRW.Rotation = quaternion.identity; 
                localTransform.ValueRW.Scale = 1f; 
            }
        }
    }
}