# ecs_planet_simulation
A planet simulation with random generated satellites made using Unity ECS and Job system for performance enhancements.


You can enter the spawning dimension and the count of satellites.


The program will calculate the target distance for each satellite with respect to the mass of the main planet and the satellite itself.


Satellites masses are created randomly in a stable interval.


After that each satellite will move to its target location to stay on the orbit of the main planet.


The main goal of this project is to maximize performance using Job System and DOTS ECS of Unity on large count of objects.

Specs:


AMD Ryzen 7 7700X 8-Core 4.5GHz
32 GB 6000Mhz Ram
NVIDIA GeForce RTX 4070 Ti


How To Use:


You can set the dimensions and the number of satellites in the space object.
(A dimension of 200x200x200 seems to be the most optimal dimension from my tests.)

Currently I can get 32 stable FPS in my simulation with 500.000 satellites each moving to its target position.

![image](https://github.com/user-attachments/assets/3e80ba35-2d0f-4457-86b2-9c142b25ab43)

