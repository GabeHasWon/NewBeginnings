using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace NewBeginnings.Common.Prim
{
    internal class TrailDrawer
    {
        public static void Draw(List<Vector2> points, float originalWidth, Color startColor, Color fadeColor)
        {
			if (points.Count <= 1)
				return;

			var _points = points.ToArray().Reverse().ToList();

			Effect effect = EffectLoader._effect;
			var device = Main.instance.GraphicsDevice;

			//calculate trail's length
			float trailLength = 0f;
			for (int i = 1; i < _points.Count; i++)
				trailLength += Vector2.Distance(_points[i - 1], _points[i]);

			//Create vertice array, needs to be equal to the number of quads * 6 (each quad has two tris, which are 3 vertices)
			int currentIndex = 0;
			var vertices = new VertexPositionColorTexture[(_points.Count - 1) * 6];

			//method to make it look less horrible
			void AddVertex(Vector2 position, Color color, Vector2 uv)
			{
				vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
			}

			float currentDistance = 0f;
			float halfWidth = originalWidth * 0.5f;

			Vector2 startNormal = CurveNormal(_points, 0);
			Vector2 prevClockwise = _points[0] + startNormal * halfWidth;
			Vector2 prevCClockwise = _points[0] - startNormal * halfWidth;

			Color previousColor = Color.White;// _trailColor.GetColourAt(0f, trailLength, _points);

			//_trailCap.AddCap(vertices, ref currentIndex, previousColor, _points[0], startNormal, _widthStart);
			for (int i = 1; i < _points.Count; i++)
			{
				currentDistance += Vector2.Distance(_points[i - 1], _points[i]);

				float thisPointsWidth = halfWidth * (1f - i / (float)(_points.Count - 1));

				Vector2 normal = CurveNormal(_points, i);
				Vector2 clockwise = _points[i] + normal * thisPointsWidth;
				Vector2 cclockwise = _points[i] - normal * thisPointsWidth;
				Color color = Color.Lerp(startColor, fadeColor, currentDistance / trailLength);// _trailColor.GetColourAt(currentDistance, trailLength, _points);

				AddVertex(clockwise, color, Vector2.UnitX * i);
				AddVertex(prevClockwise, previousColor, Vector2.UnitX * (i - 1));
				AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));

				AddVertex(clockwise, color, Vector2.UnitX * i);
				AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));
				AddVertex(cclockwise, color, new Vector2(i, 1f));

				prevClockwise = clockwise;
				prevCClockwise = cclockwise;
				previousColor = color;
			}

			//set effect parameter for matrix (todo: try have this only calculated when screen size changes?)
			int width = device.Viewport.Width;
			int height = device.Viewport.Height;
			Vector2 zoom = Main.GameViewMatrix.Zoom;
			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
			var projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			effect.Parameters["WorldViewProjection"].SetValue(view * projection);
			//effect.Parameters["WorldViewProjection"].SetValue(Main.GameViewMatrix.TransformationMatrix * Main.GameViewMatrix.ZoomMatrix);

			//apply this trail's shader pass and draw
			effect.CurrentTechnique.Passes.First().Apply();
			device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, (_points.Count - 1) * 2);
		}

		private static Vector2 CurveNormal(List<Vector2> points, int index)
		{
			if (points.Count == 1) return points[0];

			if (index == 0)
				return Clockwise90(Vector2.Normalize(points[1] - points[0]));
			if (index == points.Count - 1)
				return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
			return Clockwise90(Vector2.Normalize(points[index + 1] - points[index - 1]));
		}

		private static Vector2 Clockwise90(Vector2 vector) => new Vector2(-vector.Y, vector.X);
	}
}
