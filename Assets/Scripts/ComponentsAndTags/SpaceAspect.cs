using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlanetSimulator
{
    public readonly partial struct SpaceAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<SpaceProperties> _spaceProperties;
        private readonly RefRW<SpaceRandom> _spaceRandom;
        private readonly RefRO<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;

        public int NumberSatellites => _spaceProperties.ValueRO.NumberOfSatellites;
        public Entity SatellitePrefab => _spaceProperties.ValueRO.SatellitePrefab;

        public LocalTransform GetRandomSatelliteTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = quaternion.identity,
                Scale = 1,
            };
        }

        public float GetRandomMass()
        {
            
            return _spaceRandom.ValueRW.Value.NextFloat(5.0e18f, 5.0e21f);
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            float maxRadius = math.length(HalfDimensions);
            float minRadius = 5f;

            // Randomly select a radius
            float radius = _spaceRandom.ValueRW.Value.NextFloat(minRadius, maxRadius);

            // Generate random theta from 0 to 2π
            float theta = _spaceRandom.ValueRW.Value.NextFloat() * math.PI * 2;

            // Generate random phi using a different method to prevent clustering
            float u = _spaceRandom.ValueRW.Value.NextFloat(); // Uniformly distributed
            float phi = math.acos(1 - 2 * u); // Resulting phi from 0 to π

            // Calculate Cartesian coordinates
            float x = radius * math.sin(phi) * math.cos(theta);
            float y = radius * math.sin(phi) * math.sin(theta);
            float z = radius * math.cos(phi);

            randomPosition = Transform.Position + new float3(x, y, z);
            return randomPosition;
        }

        private float3 HalfDimensions => new()
        {
            x = _spaceProperties.ValueRO.SpaceDimensions.x / 2f,
            y = _spaceProperties.ValueRO.SpaceDimensions.y / 2f,
            z = _spaceProperties.ValueRO.SpaceDimensions.z / 2f
        };
    }
}
