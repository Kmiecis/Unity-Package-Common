using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common
{
	[Serializable]
	public struct Range3Int : IEquatable<Range3Int>
	{
		public Vector3Int min;
		public Vector3Int max;

		public static readonly Range3Int Zero;

		public static readonly Range3Int Max = new Range3Int(
			Vector3Int.one * int.MinValue,
			Vector3Int.one * int.MaxValue
		);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Range3Int(Vector3Int min, Vector3Int max)
		{
			this.min = min;
			this.max = max;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Range3Int(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			this.min = new Vector3Int(minX, minY, minZ);
			this.max = new Vector3Int(maxX, maxY, maxZ);
		}

		public Vector3 Center
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return Mathx.Multiply((max + min), 0.5f); }
		}

		public Vector3Int Extents
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return max - min; }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(int x, int y, int z)
		{
			return (
				min.x <= x &&
				min.y <= y &&
				min.z <= z &&
				x <= max.x &&
				y <= max.y &&
				z <= max.z
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector3Int v)
		{
			return Contains(v.x, v.y, v.z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Range3Int other)
		{
			return (
				Mathx.AreLesserOrEqual(min, other.min) &&
				Mathx.AreLesserOrEqual(other.max, max)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(Range3Int other)
		{
			return (
				Mathx.AreLesserOrEqual(min, other.max) &&
				Mathx.AreLesserOrEqual(other.min, max)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Range3Int Intersection(Range3Int other)
		{
			return new Range3Int(
				Mathx.Max(min, other.min),
				Mathx.Min(max, other.max)
			);
		}

		public bool Equals(Range3Int other)
		{
			return (
				Mathx.AreEqual(min, other.min) &&
				Mathx.AreEqual(max, other.max)
			);
		}

		public override bool Equals(object obj)
		{
			return obj is Range3Int && Equals((Range3Int)obj);
		}

		public override int GetHashCode()
		{
			return min.GetHashCode() ^ (max.GetHashCode() << 2);
		}

		public override string ToString()
		{
			return string.Format("Range3Int({0}, {1})", min, max);
		}
	}
}