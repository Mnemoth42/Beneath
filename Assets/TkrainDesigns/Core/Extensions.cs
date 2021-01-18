using UnityEngine;

namespace TkrainDesigns.Core
{
    /// <summary>
    /// Useful extensions for common mathmatical functions.  Many of these functions simply invoke Mathf functions,
    /// but they can now be called by simply calling the extension method on the value... example: int i=4; float f=1/i.F() instead of
    /// int i=4, float f=1/(float)i;
    /// </summary>
    public static class Extensions
    {
        // Whenever possible, this class tries to stick to simply pure functions.
        // Pure rules:  
        // 1) No variables accessed from outside sources, only what's in the parameters.
        // 2) No caching of variables, i.e., whenever possible, no creating temporary variables.
        // 3) Reuse of class statics functions like Mathf is allowed (as most of these are actually pure functions).
        // 4) The function should do exactly what it says it will.  Results should be predictable and repeatable.
        // 5) Where possible, no loops.  Recursion is allowed.

        /// <summary>
        /// Returns the absolute value of this integer.  
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int Abs(this int i)
        {
            return Mathf.Abs(i);
        }

        /// <summary>
        /// Returns the absolute value of this float. 
        /// </summary>
        /// <param name="f"></param>
        /// <returns>The absolute value of f</returns>


        public static float Abs(this float f)
        {
            return Mathf.Abs(f);
        }
        /// <summary>
        /// Returns the sign of i as an int. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int Sign(this int i)
        {
            return Mathf.Sign(i).I();
        }
        /// <summary>
        /// Returns the sign of f as a float.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sign(this float f)
        {
            return Mathf.Sign(f);
        }
        /// <summary>
        /// Returns the integer part of this float. e.g. (int)f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int I(this float f)
        {
            return (int)f;
        }
        /// <summary>
        /// Returns this int converted to float value.  e.g. (float)i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static float F(this int i)
        {
            return (float)i;
        }
        /// <summary>
        /// If f is lower than min returns min, otherwise f
        /// </summary>
        /// <param name="f"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static float Floor(this float f, float min)
        {
            return Mathf.Max(f, min);
        }
        /// <summary>
        /// If f is greater than max, returns max, otherwise f
        /// </summary>
        /// <param name="f"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Ceil(this float f, float max)
        {
            return Mathf.Min(f, max);
        }
        /// <summary>
        /// Clamps value of f between min and max.  
        /// </summary>
        /// <param name="f"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(this float f, float min, float max)
        {
            return Mathf.Clamp(f, min, max);
        }
        /// <summary>
        /// If i is lower than min return min, otherwise i
        /// </summary>
        /// <param name="i"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int Floor(this int i, int min)
        {
            return Mathf.Max(i, min);
        }
        /// <summary>
        /// if i is greater than max, return max, otherwise i
        /// </summary>
        /// <param name="i"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Ceil(this int i, int max)
        {
            return Mathf.Min(i, max);
        }
        /// <summary>
        /// Clamps i's value between min and max
        /// </summary>
        /// <param name="i"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>

        public static int Clamp(this int i, int min, int max)
        {
            return Mathf.Clamp(i, min, max);
        }
        /// <summary>
        /// A simple way of adding two ints and moduloing the result.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="adder"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>

        public static int AddMod(this int i, int adder, int modulo)
        {
            return (i + adder) % modulo;
        }

        /// <summary>
        /// Subtracts subber from i and % modulo
        /// </summary>
        /// <param name="i"></param>
        /// <param name="subber"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int SubMod(this int i, int subber, int modulo)
        {
            return (i - subber) % modulo;
        }

        /// <summary>
        /// Increments i, and applies result % modulo.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int IncMod(this int i, int modulo)
        {
            return (i + 1) % modulo;
        }

        /// <summary>
        /// Decrements i and applies result % modulo
        /// </summary>
        /// <param name="i"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int DecMod(this int i, int modulo)
        {
            return (i - 1) % modulo;
        }

        /// <summary>
        /// Returns the sine of f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sin(this float f)
        {
            return Mathf.Sin(f);
        }
        /// <summary>
        /// returns the Cosine of f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Cos(this float f)
        {
            return Mathf.Cos(f);
        }
        /// <summary>
        /// Returns true if i is any value other than zero.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool Bool(this int i)
        {
            return (i != 0);
        }

        /// <summary>
        /// Returns the distance from this vector to target Vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Distance(this Vector3 v, Vector3 target)
        {
            return Vector3.Distance(v, target);
        }

        /// <summary>
        /// Returns this Vector, replacing value of z
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 SubZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Returns this vector, replacing value of y
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 SubY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        /// <summary>
        /// Returns this vector, replacing value of x
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 SubX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        /// <summary>
        /// Returns this vector with v.x adjusted by x
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 AddX(this Vector3 v, float x)
        {
            return new Vector3(v.x + x, v.y, v.z);
        }

        /// <summary>
        /// Returns this vector with v.x adjusted by x
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 AddY(this Vector3 v, float y)
        {
            return new Vector3(v.x, v.y + y, v.z);
        }

        /// <summary>
        /// Returns this vector with v.x adjusted by x
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Vector3 AddZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, v.z + z);
        }

        /// <summary>
        /// Returns a vector facing target 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 FaceTowards(this Vector3 v, Vector3 target)
        {
            return target - v;
        }

        /// <summary>returns a vector facing away from target</summary>
        /// <param name="v"></param>
        /// <param name="target">Target to face away from.</param>
        /// <returns>v - target;</returns>
        public static Vector3 FaceAway(this Vector3 v, Vector3 target)
        {
            return v - target;
        }
        /// <summary>Shortcut to return a Quaternion from a Vector3.  Equivilant to Quaternion.Euler(v)</summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(this Vector3 v)
        {
            return Quaternion.Euler(v);
        }

        public static Quaternion LookRotation(this Vector3 v)
        {
            return Quaternion.LookRotation(v);
        }
        /// <summary>
        /// Returns this float multiplied by Time.deltaTime.  Shortcut to f*Time.deltaTime. Example: <c>Counter=speed.Frame()</c>;
        /// 
        /// </summary>
        /// <remarks>
        /// f=Time.deltaTime;</remarks>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Frame(this float f)
        {
            return f * Time.deltaTime;
        }


    }
}