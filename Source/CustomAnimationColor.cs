using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using UnityEngine;

namespace ColorfulCadas
{
    class CustomAnimationColor
    {
        private static float GetWingRotationSpeed(CicadaGraphics self, float timeStacker)
        {
            return Vector3.Slerp(self.lastZRotation, self.zRotation, timeStacker).x;
        }
        public static void CustomDrawSprites(On.CicadaGraphics.orig_DrawSprites orig, CicadaGraphics self,
            RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            for (int k = 0; k<2; k++)
            {
                for(int j = 0; j<2; j++)
                {
                    float vel = GetWingRotationSpeed(self, timeStacker);
                    sLeaser.sprites[self.WingSprite(k, j)].color = Color.Lerp(new Color(0f, 0f, 0f), self.shieldColor, Mathf.Abs(vel) + 0.2f);
                }
            }
        }
    }
}
