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
            
            return _spaceRandom.ValueRW.Value.NextFloat(5.0e19f, 5.0e22f);
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            float maxRadius = math.length(HalfDimensions); 
            float minRadius = 5f; 

            float radius = _spaceRandom.ValueRW.Value.NextFloat(minRadius, maxRadius);

            float theta = _spaceRandom.ValueRW.Value.NextFloat() * math.PI * 2; 
            float phi = _spaceRandom.ValueRW.Value.NextFloat() * math.PI; 

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
