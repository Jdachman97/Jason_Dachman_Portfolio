using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 7500f;
        public const float PlayfieldSizeY = 6000f;
        public const float shipSpeed = 1200.0f;
        public const float shipRotateSpeed = 3.0f;
        //asteroid constants
        public const int NumAsteroids = 15;

        public const float AsteroidMinSpeed = 600.0f;
        public const float AsteroidMaxSpeed = 800.0f;

        public const float AsteroidSpeedAdjustment = 5.0f;

        public const float AsteroidBoundingSphereScale = 0.9f;  //95% size
        public const float ShipBoundingSphereScale = 0.35f;
        public const int NumBullets = 30;
        public const float BulletSpeedAdjustment = 1500.0f;

        public const int ShotPenalty = 1;
        public const int DeathPenalty = 100;
        public const int WarpPenalty = 50;
        public const int KillBonus = 25;
    }
}