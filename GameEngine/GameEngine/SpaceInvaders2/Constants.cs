using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class Constants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 12800f;
        public const float PlayfieldSizeY = 12000f;
        public const float shipSpeed = 5.0f;
        public const float shipRotateSpeed = 3.0f;
        //asteroid constants
        public const int NumEnemies = 50;
        public const int enemySpacerX = 2300;
        public const int enemySpacerY = 2000;


        public const float enemyBulletSpeed = 1800.0f;


        public const float EnemyBoundingSphereScale = 0.9f;  //95% size
        public const float ShipBoundingSphereScale = 0.4f;
        public const int NumBullets = 600;
        public const float BulletSpeedAdjustment = 6.0f;

        public const int ShotPenalty = 1;
        public const int DeathPenalty = 1000;
        public const int WarpPenalty = 50;
        public const int KillBonus = 25;

        public const int shieldSpacerX = 5000;
        public const int shieldSpacerY = 400;
        public const int numShields = 9;
    }
}