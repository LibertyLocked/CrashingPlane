﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrashingPlane
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Gets the angle of a vector (in degrees). Zero pointing up, increasing clockwise.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToDegrees(this Vector2 vector)
        {
            float radians = ToRadians(vector);
            return MathHelper.ToDegrees(radians);
        }

        /// <summary>
        /// Gets the angle of a vector (in radians). Zero pointing up, increasing clockwise.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToRadians(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        /// <summary>
        /// Gets the normal vector that has the angle.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector2 RadiansToVector(float rad)
        {
            return new Vector2((float)Math.Sin(rad), -(float)Math.Cos(rad));
        }

        /// <summary>
        /// Gets the rotated vector from the original vector.
        /// </summary>
        /// <param name="originalVector"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static Vector2 RotateVector(this Vector2 originalVector, float radians)
        {
            float length = originalVector.Length();
            Vector2 newVector = RadiansToVector(originalVector.ToRadians() + radians) * length;
            return newVector;
        }
    }
}
