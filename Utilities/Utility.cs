﻿using System;

namespace Common
{
	public static class Utility
	{
		public static void AssignOrThrow<T>(ref T target, T value)
		{
			if (target != null)
			{
				throw new Exception(string.Format("Target {0} already assigned", typeof(T).Name));
			}
			target = value;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}
	}
}