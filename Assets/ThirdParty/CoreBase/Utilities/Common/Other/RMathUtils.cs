/**
 *  Created by hnim.
 *  Copyright (c) 2017  All rights reserved.
 */
using UnityEngine;
namespace Utilities.Common.Obsolate
{
    public class RMathUtils
    {
        public static float SinRad(float pRad)
        {
            return Mathf.Sin(pRad);
        }
        public static float CosRad(float pRad)
        {
            return Mathf.Cos(pRad);
        }
        public static float SinDeg(float pDeg)
        {
            return Mathf.Sin(DegToRad(pDeg));
        }
        public static float CosDeg(float pDeg)
        {
            return Mathf.Cos(DegToRad(pDeg));
        }
        public static float TanDeg(float pDeg)
        {
            return Mathf.Tan(DegToRad(pDeg));
        }
        public static float DegToRad(float pDeg)
        {
            return pDeg * Mathf.Deg2Rad;
        }
        public static float TadToDeg(float pRad)
        {
            return pRad * Mathf.Rad2Deg;
        }
        public static float AngleDeg(Vector2 pFrom, Vector2 pTo)
        {
            return AtanDeg(pTo.y - pFrom.y, pTo.x - pFrom.x);
        }
        public static float AtanDeg(float dy, float dx)
        {
            return TadToDeg(AtanRad(dy, dx));
        }
        public static float AtanRad(float dy, float dx)
        {
            return Mathf.Atan2(dy, dx);
        }
        public static bool IsPointInsideEllipse(Vector2 pointToCheck, Vector2 ellipsePos, Vector2 ellipseSize)
        {
            return IsPointInsideEllipse(pointToCheck, ellipsePos, ellipseSize.x, ellipseSize.y);
        }
        public static bool IsPointInsideEllipse(Vector2 pointToCheck, Vector2 ellipsePos, float ellipseWidth, float ellipseHeight)
        {
            if (ellipseWidth <= 0 || ellipseHeight <= 0)
            {
                return false;
            }
            float dx = pointToCheck.x - ellipsePos.x;
            float dy = pointToCheck.y - ellipsePos.y;
            float xComponent = (dx * dx / (ellipseWidth * ellipseWidth));
            float yComponent = (dy * dy / (ellipseHeight * ellipseHeight));
            float value = xComponent + yComponent;

            if (value < 1)
            {
                return true;
            }
            return false;
        }
        public static bool Approximately(Vector3 pValue, Vector3 pRefValue)
        {
            bool ax = Mathf.Approximately(pValue.x, pRefValue.x);
            bool ay = Mathf.Approximately(pValue.y, pRefValue.y);
            bool az = Mathf.Approximately(pValue.z, pRefValue.z);
            return ax && ay && az;
        }
        public static bool CheckLineIntersect(Vector2 pLineStart, Vector2 pLineEnd, Rect pRect)
        {
            // check if the line has hit any of the rectangle's sides
            float x1 = pLineStart.x;
            float y1 = pLineStart.y;
            float x2 = pLineEnd.x;
            float y2 = pLineEnd.y;
            float rx = pRect.x;
            float ry = pRect.y;
            float rw = pRect.width;
            float rh = pRect.height;
            // uses the Line/Line function below
            bool left = lineLine(x1, y1, x2, y2, rx, ry, rx, ry + rh);
            bool right = lineLine(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh);
            bool top = lineLine(x1, y1, x2, y2, rx, ry, rx + rw, ry);
            bool bottom = lineLine(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh);

            // if ANY of the above are true, the line
            // has hit the rectangle
            if (left || right || top || bottom)
            {
                return true;
            }
            return false;
        }
        public static bool lineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return true;
            }
            return false;
        }
        public const int Intersection_OUT_SIDE = 0;
        public const int Intersection_IN_SIDE = 1;
        public const int Intersection_CUT = 2;
        public static int intersectEllipseLine(Vector2 center, float rx, float ry, Vector2 a1, Vector2 a2)
        {
            int result = 0;
            var origin = a1;
            var dir = a2 - a1;
            var diff = origin - center;
            var mDir = new Vector2(dir.x / (rx * rx), dir.y / (ry * ry));
            var mDiff = new Vector2(diff.x / (rx * rx), diff.y / (ry * ry));

            var a = Vector2.Dot(dir, mDir);
            var b = Vector2.Dot(dir, mDiff);
            var c = Vector2.Dot(diff, mDiff) - 1.0f;
            var d = b * b - a * c;

            if (d < 0)
            {
                result = Intersection_OUT_SIDE;
            }
            else if (d > 0)
            {
                var root = Mathf.Sqrt(d);
                var t_a = (-b - root) / a;
                var t_b = (-b + root) / a;

                if ((t_a < 0 || 1 < t_a) && (t_b < 0 || 1 < t_b))
                {
                    if ((t_a < 0 && t_b < 0) || (t_a > 1 && t_b > 1))
                        result = Intersection_OUT_SIDE;
                    else
                        result = Intersection_IN_SIDE;
                }
                else
                {
                    result = Intersection_CUT;
                }
            }
            else
            {
                var t = -b / a;
                if (0 <= t && t <= 1)
                {
                    result = Intersection_CUT;
                }
                else
                {
                    result = Intersection_OUT_SIDE;
                }
            }

            return result;
        }
        public static bool intersectEllipseRectangle(Vector2 c, float rx, float ry, Rect pRect)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);
            return intersectEllipseLine(c, rx, ry, tL, tR) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, tL, bL) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, bL, bR) == Intersection_CUT
                || intersectEllipseLine(c, rx, ry, bR, tR) == Intersection_CUT;
        }
        public static bool rectInsideEllipse(Vector2 c, float rx, float ry, Rect pRect)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);
            return intersectEllipseLine(c, rx, ry, tL, tR) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, tL, bL) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, bL, bR) == Intersection_IN_SIDE
                && intersectEllipseLine(c, rx, ry, bR, tR) == Intersection_IN_SIDE;
        }
        private static bool inAngle(Vector2 v1, Vector2 v2, float pFromDeg, float pToDeg)
        {
            float deg = AngleDeg(v1, v2);
            return (deg >= pFromDeg && deg <= pToDeg) || (deg + 360 >= pFromDeg && deg + 360 <= pToDeg);
        }
        public static bool intersectOrOverlapEllipseRectangle(Vector2 c, float rx, float ry, Rect pRect, float pFromAngle = 0, float pToAngle = 360, bool pRightSide = true, bool drawBox = false)
        {
            var tL = new Vector2(pRect.x, pRect.y + pRect.height);
            var tR = new Vector2(pRect.x + pRect.width, pRect.y + pRect.height);
            var bL = new Vector2(pRect.x, pRect.y);
            var bR = new Vector2(pRect.x + pRect.width, pRect.y);

            float a1 = pRightSide ? pFromAngle : pFromAngle + 180;
            float a2 = pRightSide ? pToAngle : pToAngle + 180;

            pFromAngle = a1;
            pToAngle = a2;

            var result = true;
            bool inAngleRange = true;
            if (pToAngle - pFromAngle != 360)
            {
                // check if any angle from center to rect point in angle range
                inAngleRange = (pRightSide ? pRect.xMax > c.x : pRect.xMin < c.x) &&
                    (inAngle(c, tL, pFromAngle, pToAngle)
                    || inAngle(c, tR, pFromAngle, pToAngle)
                    || inAngle(c, bL, pFromAngle, pToAngle)
                    || inAngle(c, bR, pFromAngle, pToAngle));
                if (inAngleRange == false)
                {
                    inAngleRange = false;
                    result = inAngleRange;
                }
            }
            if (inAngleRange)
            {
                var r1 = intersectEllipseLine(c, rx, ry, tL, tR);
                if (r1 != Intersection_CUT)
                {
                    var r2 = intersectEllipseLine(c, rx, ry, tL, bL);
                    if (r2 != Intersection_CUT)
                    {
                        var r3 = intersectEllipseLine(c, rx, ry, bL, bR);
                        if (r3 != Intersection_CUT)
                        {
                            var r4 = intersectEllipseLine(c, rx, ry, bR, tR);
                            if (r4 != Intersection_CUT)
                            {
                                result = r1 == Intersection_IN_SIDE
                                    || r2 == Intersection_IN_SIDE
                                    || r3 == Intersection_IN_SIDE
                                    || r4 == Intersection_IN_SIDE;
                            }
                        }
                    }
                }
            }
#if UNITY_EDITOR
            if (drawBox)
            {
                var resultColor = result ? Color.green : Color.red;
                if (inAngleRange == false)
                    resultColor = Color.yellow;

                DebugDraw.DrawLine(tL, tR, resultColor, 0);
                DebugDraw.DrawLine(tR, bR, resultColor, 0);
                DebugDraw.DrawLine(bR, bL, resultColor, 0);
                DebugDraw.DrawLine(bL, tL, resultColor, 0);
                //*
                float x = c.x + rx * CosDeg(a1);
                float y = c.y + ry * SinDeg(a1);
                DebugDraw.DrawLine(c, new Vector3(x, y), Color.red, 0);
                x = c.x + rx * CosDeg(a2);
                y = c.y + ry * SinDeg(a2);
                DebugDraw.DrawLine(c, new Vector3(x, y), Color.blue, 0);
                DrawEllipse(c, rx, ry, Color.red, 0);
                //*/
            }
#endif
            return result;
        }


        public static float Sum(float[] array)
        {
            float sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum += array[i];
            return sum;
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawEllipse(Vector3 center, float width, float height, Color color, float duration = 0.5f)
        {
            int steps = 100;
            float interval = 2 * width / steps;
            Vector3 previousPoint = Vector3.zero;
            Vector3 currentPoint = Vector3.zero;
            for (int i = 0; i <= steps; i++)
            {
                previousPoint = currentPoint;
                float x = -width + interval * i;
                float y = Mathf.Sqrt(1 - (x * x) / (width * width)) * height;
                currentPoint = new Vector2(x, y);
                if (i > 0)
                    DebugDraw.DrawLine(center + previousPoint, center + currentPoint, color, duration);
                if (i > 0)
                    DebugDraw.DrawLine(center + new Vector3(previousPoint.x, -previousPoint.y), center + new Vector3(currentPoint.x, -currentPoint.y), color, duration);
            }
        }
    }
}
