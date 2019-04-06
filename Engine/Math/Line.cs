using System;
using UnityEngine;

namespace Eitrum.Mathematics {
    [Serializable]
    public struct Line {
        #region Variables

        [SerializeField]
        private float p0x, p0y, p0z, p1x, p1y, p1z;

        #endregion

        #region Properties

        public Vector3 StartPoint {
            get {
                return new Vector3(p0x, p0y, p0z);
            }
        }


        public Vector3 EndPoint {
            get {
                return new Vector3(p1x, p1y, p1z);
            }
        }

        public Vector3 Direction {
            get {
                return new Vector3(Delta(0), Delta(1), Delta(2));
            }
        }

        public float Length {
            get {
                return Direction.magnitude;
            }
        }

        #endregion

        #region Constructor

        public Line(Vector3 direction) {
            p0x = 0f;
            p0y = 0f;
            p0z = 0f;
            p1x = direction.x;
            p1y = direction.y;
            p1z = direction.z;
        }

        public Line(Vector3 direction, float distance) {
            direction *= distance;
            p0x = 0f;
            p0y = 0f;
            p0z = 0f;
            p1x = direction.x;
            p1y = direction.y;
            p1z = direction.z;
        }

        public Line(Vector3 startPoint, Vector3 endPoint) {
            p0x = startPoint.x;
            p0y = startPoint.y;
            p0z = startPoint.z;
            p1x = endPoint.x;
            p1y = endPoint.y;
            p1z = endPoint.z;
        }

        public Line(Vector3 origin, Vector3 direction, float distance) {
            direction = origin + direction * distance;
            p0x = origin.x;
            p0y = origin.y;
            p0z = origin.z;
            p1x = direction.x;
            p1y = direction.y;
            p1z = direction.z;
        }

        public Line(Transform t0, Transform t1, Space space = Space.World) {
            if (space == Space.World) {
                var localP0 = t0.position;
                var localP1 = t1.position;
                p0x = localP0.x;
                p0y = localP0.y;
                p0z = localP0.z;

                p1x = localP1.x;
                p1y = localP1.y;
                p1z = localP1.z;
            }
            else {
                var localP0 = t0.localPosition;
                var localP1 = t1.localPosition;
                p0x = localP0.x;
                p0y = localP0.y;
                p0z = localP0.z;

                p1x = localP1.x;
                p1y = localP1.y;
                p1z = localP1.z;
            }
        }

        #endregion

        #region Static Constructor

        public static Line Create(Vector3 direction) {
            return new Line(direction);
        }

        public static Line Create(Vector3 direction, float distance) {
            return new Line(direction, distance);
        }

        public static Line Create(Vector3 startPoint, Vector3 endPoint) {
            return new Line(startPoint, endPoint);
        }

        public static Line Create(Vector3 origin, Vector3 direction, float distance) {
            return new Line(origin, direction, distance);
        }

        public static Line Create(Transform t0, Transform t1, Space space = Space.World) {
            return new Line(t0, t1, space);
        }

        public static Line Create<T>(T t0, T t1, Space space = Space.World) where T : Component {
            return new Line(t0.transform, t1.transform, space);
        }

        #endregion

        #region Private Help Functions

        private unsafe float Value(int index) {
            fixed (float* p = &p0x) {
                return *(p + index);
            }
        }

        private unsafe float Delta(int index) {
            fixed (float* p = &p0x) {
                return ((*(p + 3 + index)) - *(p + index));
            }
        }

        #endregion

        #region Lerp

        public Vector3 GetPointOnLine(float time) {
            return new Vector3(
                p0x + (p1x - p0x) * time,
                p0y + (p1y - p0y) * time,
                p0z + (p1z - p0z) * time
                );
        }

        public Vector3 GetPointOnLineClamped(float time) {
            if (time < 0f)
                time = 0f;
            else if (time > 1f)
                time = 1f;
            return new Vector3(
                p0x + (p1x - p0x) * time,
                p0y + (p1y - p0y) * time,
                p0z + (p1z - p0z) * time
                );
        }

        #endregion

        #region Closest Point On Line

        public float GetDistanceToLine(Vector3 point) {
            var dx = Delta(0);
            var dy = Delta(1);
            var dz = Delta(2);

            if (dx == 0f && dy == 0f && dz == 0f)
                return Vector3.Distance(StartPoint, point);

            var p = point - StartPoint;
            float t = ((p.x * dx) + (p.y * dy) + (p.z * dz)) / (dx * dx + dy * dy + dz * dz);
            return Vector3.Distance(GetPointOnLine(t), point);
        }

        public float GetDistanceToLineClamped(Vector3 point) {
            var dx = Delta(0);
            var dy = Delta(1);
            var dz = Delta(2);

            if (dx == 0f && dy == 0f && dz == 0f)
                return Vector3.Distance(StartPoint, point);

            var p = point - StartPoint;
            float t = ((p.x * dx) + (p.y * dy) + (p.z * dz)) / (dx * dx + dy * dy + dz * dz);
            return Vector3.Distance(GetPointOnLineClamped(t), point);
        }

        public Vector3 GetClosestPointOnLine(Vector3 point) {
            var dx = Delta(0);
            var dy = Delta(1);
            var dz = Delta(2);

            if (dx == 0f && dy == 0f && dz == 0f)
                return StartPoint;

            var p = point - StartPoint;
            float t = ((p.x * dx) + (p.y * dy) + (p.z * dz)) / (dx * dx + dy * dy + dz * dz);
            return GetPointOnLine(t);
        }

        public Vector3 GetClosestPointOnLineClamped(Vector3 point) {
            var dx = Delta(0);
            var dy = Delta(1);
            var dz = Delta(2);

            if (dx == 0f && dy == 0f && dz == 0f)
                return StartPoint;

            var p = point - StartPoint;
            float t = ((p.x * dx) + (p.y * dy) + (p.z * dz)) / (dx * dx + dy * dy + dz * dz);
            return GetPointOnLineClamped(t);
        }

        public float GetValueFromPointOnLine(Vector3 point) {
            var dx = Delta(0);
            var dy = Delta(1);
            var dz = Delta(2);

            if (dx == 0f && dy == 0f && dz == 0f)
                return Vector3.Distance(point, StartPoint);

            var p = StartPoint - point;
            float t = ((p.x * dx) + (p.y * dy) + (p.z * dz)) / (dx * dx + dy * dy + dz * dz);
            return t;
        }

        #endregion

        #region Intersection

        /// <summary>
        /// Not IMPLEMENTED
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public Vector3? GetPointAtIntersection(Plane plane) {
            var dir = Direction;
            var dotPlaneDir = Vector3.Dot(plane.normal, dir);
            if (dotPlaneDir == 0f) {
                return null;
            }
            float f = (Vector3.Dot(plane.normal, plane.normal * plane.distance) - Vector3.Dot(plane.normal, StartPoint)) / dotPlaneDir;
            return StartPoint + (dir * f);
        }

        public Vector3? GetPointAtIntersection(int index, float value) {
            var dir = Direction;
            var val = Delta(index);
            if (val == 0f)
                return null;

            var sVal = Value(index);
            dir /= Mathf.Abs(val);
            value -= sVal;

            if (val < 0f)
                return new Vector3(
                    p0x + (dir.x * -value),
                    p0y + (dir.y * -value),
                    p0z + (dir.z * -value));

            return new Vector3(
                p0x + (dir.x * value),
                p0y + (dir.y * value),
                p0z + (dir.z * value));
        }

        public Vector3? GetPointAtIntersectionX(float value) {
            return GetPointAtIntersection(0, value);
        }

        public Vector3? GetPointAtIntersectionY(float value) {
            return GetPointAtIntersection(1, value);
        }

        public Vector3? GetPointAtIntersectionZ(float value) {
            return GetPointAtIntersection(2, value);
        }

        #endregion

        #region Line-to-line

        /// <summary>
        /// NOT WORKING
        /// </summary>
        /// <param name="line0"></param>
        /// <param name="line1"></param>
        /// <returns></returns>
        public static Line ClosestDistanceBetweenLines(Line line0, Line line1) {
            var dir0 = line0.Direction;
            var dir1 = line1.Direction;

            if (dir0 == dir1)
                return new Line(line0.StartPoint, line1.StartPoint);



            // FIX THIS CODE
            return line0;
        }

        #endregion
    }
}