using PlanetSimulator;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct PlanetPositionUpdateSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlanetProperties>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var planetEntity = SystemAPI.GetSingletonEntity<PlanetProperties>();
            var planetProperties = SystemAPI.GetComponent<PlanetProperties>(planetEntity);

            SystemAPI.SetComponent(planetEntity, new LocalTransform
            {
                Position = planetProperties.position,  
                Rotation = quaternion.identity,       
                Scale = 10f                            
            });
        }
    }
}