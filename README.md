# 1. Project Context	

This project was created for UtinComputer Test Task on a Middle Unity Developer position.

Projecr preview: https://youtube.com/shorts/-627bO_erTc?si=145m564jcKSrJr-6

Controls:  
- Hold tap: charge shot (mass transfers into projectile)  
- Release: shoot towards target

# 2. Key Features

- Shot system
- Mass charging
- Explosiding obstacles like infection
- Adaptive difficulty: shot size directly affects path width and traversal ability
- Resource management mechanic: player mass as both health and ammo

# 3. Technical Highlights

- Minimum GameObjects, all controllers are pure classes
- Data-driven PlayerConfig
- MVC principle in all controllers
- Custom Gizmos debugging system (Sphere, SphereCast) usable from pure classes
- Visual debugging for physics queries and gameplay logic
- Decoupled gameplay systems (Player / Shot / Infection / Movement)
- Clear separation between simulation and presentation layers
- Constructor-based dependency injection

# 4. Performance metrics

- Platform: Android;
- Render pipeline: URP (3D)
- Size .APK build - 60,7MB (LZ4);
- Peak GC alloc: 80 B/frame
- Peak SetPass Calls: 60;
- Peak Draw calls: 50;
- Target frame rate: 60 FPS
- Test device: MediaTek Helio G90T (mid-range Android device)

# 5. Architecture Overview

Core systems:
- UnityEntryPoint - single entry point;
- ShotManager - control ShotControllers, which control model Shot(Pure class) and view ShotView(Monobehaviour);
- PlayerController - control PlayerView movement and charging Shots by itself mass;
- GameLoop - game flow like victory and defeat;
- InfectionController - control infect and spread infection, and further exposion infectable;

# 6. Limitations

- Complexity of dependecy injection (in thic case it is enough, but in the future should use DI Container)
- Handle placing obstacles;

# 7. Lessons Learned

- Importance of SRP and system boundaries
- Maintaince of MVC;
