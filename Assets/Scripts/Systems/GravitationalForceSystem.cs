using PlanetSimulator;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct GravitationalForceAndTransformSystem : ISystem
    {
        private const float GravitationalConstant = 6.67430e-11f;
        private const float ConstantVelocity = 10f;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlanetProperties>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var planetEntity = SystemAPI.GetSingletonEntity<PlanetProperties>();
            var planet = SystemAPI.GetComponent<PlanetProperties>(planetEntity);
            var planetPosition = planet.position;
            var planetMass = planet.mass;

            float deltaTime = SystemAPI.Time.DeltaTime;

            var moveJob = new MoveSatellitesAndUpdateTransformJob
            {
                PlanetPosition = planetPosition,
                PlanetMass = planetMass,
                GravitationalConstant = GravitationalConstant,
                DeltaTime = deltaTime,
                ConstantVelocity = ConstantVelocity,
                StoppingDistance = 0.1f
            };

            var moveJobHandle = moveJob.ScheduleParallel(state.Dependency);
            state.Dependency = moveJobHandle;
        }

        [BurstCompile]
        public partial struct MoveSatellitesAndUpdateTransformJob : IJobEntity
        {
            public float3 PlanetPosition;
            public float PlanetMass;
            public float GravitationalConstant;
            public float DeltaTime;
            public float ConstantVelocity;
            public float StoppingDistance;

            public void Execute(ref SatelliteProperties satellite, ref LocalTransform transform)
            {
                float satelliteMass = satellite.mass;

                // Calculate desired orbit radius based on satellite mass and gravitational force
                float desiredOrbitRadius = math.sqrt((GravitationalConstant * PlanetMass) / satelliteMass);

                float3 directionToPlanet = math.normalize(satellite.position - PlanetPosition);

                float3 targetPosition = PlanetPosition + directionToPlanet * desiredOrbitRadius;
                float3 direction = targetPosition - satellite.position;
                float distanceToTarget = math.length(direction);

                if (distanceToTarget < StoppingDistance)
                {
                    satellite.position = targetPosition;
                }
                else
                {
                    direction = math.normalize(direction);
                    satellite.position += direction * ConstantVelocity * DeltaTime;
                }

                transform.Position = satellite.position;
                transform.Rotation = quaternion.identity;
                transform.Scale = 1f;
            }
        }
    }
}
