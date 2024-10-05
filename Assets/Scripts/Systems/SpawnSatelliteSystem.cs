using PlanetSimulator;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnSatelliteSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpaceProperties>();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var spaceEntity = SystemAPI.GetSingletonEntity<SpaceProperties>();
            var space = SystemAPI.GetAspect<SpaceAspect>(spaceEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            for (var i = 0; i < space.NumberSatellites; i++)
            {
                var newSattelite = ecb.Instantiate(space.SatellitePrefab);
                var newSatelliteTransform = space.GetRandomSatelliteTransform();
                ecb.SetComponent(newSattelite,new SatelliteProperties
                {
                    position = newSatelliteTransform.Position,
                    velocity = float3.zero,
                    mass = space.GetRandomMass()
                });
            }
            ecb.Playback(state.EntityManager);
        }
    }
}