using PlanetSimulator;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct GravitationalForceSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlanetProperties>();
        }

        private const float GravitationalConstant = 6.67430e-11f; 
        private const float ConstantVelocity = 10f; 

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var planetEntity = SystemAPI.GetSingletonEntity<PlanetProperties>();
            var planet = SystemAPI.GetComponent<PlanetProperties>(planetEntity);
            var planetPosition = planet.position;
            var planetMass = planet.mass;

            var satelliteQuery = SystemAPI.QueryBuilder().WithAll<SatelliteProperties>().Build();
            var satelliteCount = satelliteQuery.CalculateEntityCount();

            var positions = new NativeArray<float3>(satelliteCount, Allocator.TempJob);
            var targetPositions = new NativeArray<float3>(satelliteCount, Allocator.TempJob);
            var masses = new NativeArray<float>(satelliteCount, Allocator.TempJob);

            int index = 0;
            foreach (var satellite in SystemAPI.Query<SatelliteProperties>())
            {
                positions[index] = satellite.position;
                masses[index] = satellite.mass; 
                index++;
            }

            var targetJob = new CalculateTargetPositionsJob
            {
                Positions = positions,
                TargetPositions = targetPositions,
                PlanetPosition = planetPosition,
                PlanetMass = planetMass,
                GravitationalConstant = GravitationalConstant,
                SatelliteMasses = masses
            };

            var targetJobHandle = targetJob.Schedule(satelliteCount, 64, state.Dependency);
            targetJobHandle.Complete();
            
            var moveJob = new MoveSatellitesJob
            {
                Positions = positions,
                TargetPositions = targetPositions,
                DeltaTime = SystemAPI.Time.DeltaTime,
                ConstantVelocity = ConstantVelocity,
                StoppingDistance = 0.1f
            };

            var moveJobHandle = moveJob.Schedule(satelliteCount, 64, targetJobHandle);
            moveJobHandle.Complete();
            
            index = 0;
            foreach (var (satellite, entity) in SystemAPI.Query<SatelliteProperties>().WithEntityAccess())
            {
                var satelliteRef = SystemAPI.GetComponentRW<SatelliteProperties>(entity);
                satelliteRef.ValueRW.position = positions[index]; 
                index++;
            }

            positions.Dispose();
            targetPositions.Dispose();
            masses.Dispose();
        }
    }

    [BurstCompile]
    public struct CalculateTargetPositionsJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<float3> TargetPositions;
        public float3 PlanetPosition;
        public float PlanetMass;
        public float GravitationalConstant;
        [ReadOnly] public NativeArray<float> SatelliteMasses;

        public void Execute(int index)
        {
            float satelliteMass = SatelliteMasses[index];

            float desiredOrbitRadius = math.sqrt((GravitationalConstant * PlanetMass) / satelliteMass);

            float3 directionToPlanet = math.normalize(Positions[index] - PlanetPosition);

            TargetPositions[index] = PlanetPosition + directionToPlanet * desiredOrbitRadius;
        }
    }

    [BurstCompile]
    public struct MoveSatellitesJob : IJobParallelFor
    {
        public NativeArray<float3> Positions;
        [ReadOnly] public NativeArray<float3> TargetPositions;
        public float DeltaTime;
        public float ConstantVelocity;
        public float StoppingDistance;

        public void Execute(int index)
        {
            float3 direction = TargetPositions[index] - Positions[index];
            float distanceToTarget = math.length(direction);
        
            if (distanceToTarget < StoppingDistance)
            {
                Positions[index] = TargetPositions[index];
            }
            else
            {
                direction = math.normalize(direction);
                Positions[index] += direction * ConstantVelocity * DeltaTime;
            }
        }
    }

}
