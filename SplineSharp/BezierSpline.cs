﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SplineSharp
{
    public class BezierSpline
    {
        private const int LineSteps = 10;

        public Transform[] points;

        private int CurveCount
        {
            get { return (points.Length - 1) / 3; }
        }

        public Transform TryGetTransformFromPosition(Vector2 position)
        {
            if (points.Any(x => x.TryGetPosition(position))) return points.First(x => x.TryGetPosition(position));

            return null;
        }

        public Vector2 GetPoint(float t, int curveIndex)
        {
            return Bezier.GetPoint(
                points[0 + (curveIndex * 3)].Position, 
                points[1 + (curveIndex * 3)].Position, 
                points[2 + (curveIndex * 3)].Position, 
                points[3 + (curveIndex * 3)].Position, t);
        }

        public Vector2 GetVelocity(float t)
        {
            Vector2 Velocity = Vector2.Zero;

            Velocity = Bezier.GetFirstDerivative(points[0].Position, points[1].Position, points[2].Position, points[3].Position, t);
            Velocity.Normalize();
            return Velocity;
        }

        public void AddCurveLeft()
        {
            Transform point = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);
            
            points[points.Length - 3] = new Transform(new Vector2(point.Position.X + 40f, point.Position.Y - 0f));            
            points[points.Length - 2] = new Transform(new Vector2(point.Position.X + 40f, point.Position.Y - 80f));            
            points[points.Length - 1] = new Transform(new Vector2(point.Position.X + 0f, point.Position.Y - 80f));
        }

        public void AddCurveRight()
        {
            Transform point = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);

            points[points.Length - 3] = new Transform(new Vector2(point.Position.X - 40f, point.Position.Y - 0f));
            points[points.Length - 2] = new Transform(new Vector2(point.Position.X - 40f, point.Position.Y - 80f));
            points[points.Length - 1] = new Transform(new Vector2(point.Position.X - 0f, point.Position.Y - 80f));
        }

        public void DrawSpline(SpriteBatch spriteBatch)
        {
            if (Setup.Pixel == null)
            {
                throw new Exception("You need to initialize the SplineSharp library first by calling 'SplineSharp.Setup.Initialize();'");
            }

            if (points.Length <= 1 || points.ToList().TrueForAll(x => x.Equals(Vector2.Zero))) return;

            float distance = 0, angle = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (i + 1 > points.Length - 1)
                {
                    DrawPoint(spriteBatch, points[i].Position, angle);
                    break;
                }

                distance = Vector2.Distance(points[i].Position, points[i + 1].Position);
                angle = (float)Math.Atan2(points[i + 1].Position.Y - points[i].Position.Y, points[i + 1].Position.X - points[i].Position.X);

                DrawLine(spriteBatch, points[i].Position, angle, distance, Setup.BaseLineColor, Setup.BaseLineThickness);
                DrawPoint(spriteBatch, points[i].Position, angle);
            }

            Vector2 lineStart = GetPoint(0f, 0);
            for (int j = 0; j < CurveCount; j++)
            {
                for (int i = 1; i <= LineSteps; i++)
                {
                    Vector2 lineEnd = GetPoint(i / (float)LineSteps, j);

                    float distanceStep = Vector2.Distance(lineStart, lineEnd);
                    float angleStep = (float)Math.Atan2(lineEnd.Y - lineStart.Y, lineEnd.X - lineStart.X);

                    DrawLine(spriteBatch, lineStart, angleStep, distanceStep, Setup.CurveLineColor, Setup.CurveLineThickness);

                    if (Setup.ShowVelocityVectors)
                    {
                        DrawLine(spriteBatch, lineEnd + GetVelocity(i / (float)LineSteps), angleStep,
                            Setup.VelocityLineLength, Setup.VelocityLineColor, Setup.VelocityLineThickness);
                    }

                    lineStart = lineEnd;
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 position, float angle, float distance, Color color, float thickness)
        {
            spriteBatch.Draw(Setup.Pixel,
                             position,
                             null,
                             color,
                             angle,
                             Vector2.Zero,
                             new Vector2(distance, thickness),
                             SpriteEffects.None,
                             0);
        }

        private void DrawPoint(SpriteBatch spriteBatch, Vector2 point, float angle)
        {
            spriteBatch.Draw(Setup.Pixel,
                             point,
                             null,
                             Setup.PointColor,
                             angle,
                             new Vector2(0.5f),
                             Setup.PointThickness,
                             SpriteEffects.None,
                             0f);
        }


        public void Reset()
        {
            points = new Transform[]
            {
                new Transform(new Vector2(50, 50)),
                new Transform(new Vector2(300, 50)),
                new Transform(new Vector2(50, 300)),
                new Transform(new Vector2(300, 300))
            };
        }
    }
}