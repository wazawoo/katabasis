// Copyright (c) BottlenoseLabs (https://github.com/bottlenoselabs). All rights reserved.
// Licensed under the MS-PL license. See LICENSE file in the Git repository root directory for full license information.
using System.Numerics;

namespace Katabasis.Samples
{
	internal struct Vertex : IVertexType
	{
		public Vector3 Position;
		public Color Color;

		public static readonly VertexDeclaration Declaration;

		VertexDeclaration IVertexType.VertexDeclaration => Declaration;

		static Vertex()
		{
			var elements = new[]
			{
				new VertexElement(
					0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(
					12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
			};

			Declaration = new VertexDeclaration(elements);
		}
	}
}
