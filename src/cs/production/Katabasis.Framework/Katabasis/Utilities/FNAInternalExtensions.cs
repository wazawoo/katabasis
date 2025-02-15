// Copyright (c) BottlenoseLabs (https://github.com/bottlenoselabs). All rights reserved.
// Licensed under the MS-PL license. See LICENSE file in the Git repository root directory for full license information.
using System;
using System.IO;
using System.Reflection;

namespace Katabasis
{
	internal static class FNAInternalExtensions
	{
		private static readonly FieldInfo? _fieldMemoryStream =
			// .NET
			typeof(MemoryStream).GetField("_exposable", BindingFlags.NonPublic | BindingFlags.Instance) ??
			// Old Mono
			typeof(MemoryStream).GetField("allowGetBuffer", BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary>
		///     Returns the array of unsigned bytes from which this stream was created.
		///     The return value indicates whether the conversion succeeded.
		///     This is similar to .NET 4.6's TryGetBuffer.
		/// </summary>
		/// <param name="stream">The stream to get the buffer from.</param>
		/// <param name="buffer">The byte array from which this stream was created.</param>
		/// <returns><b>true</b> if the conversion was successful; otherwise, <b>false</b>.</returns>
		internal static bool TryGetBuffer(this MemoryStream stream, out byte[] buffer)
		{
			// Check if the buffer is public by reflecting into a known internal field.
			if (_fieldMemoryStream != null)
			{
				if ((bool)(_fieldMemoryStream.GetValue(stream) ?? false))
				{
					buffer = stream.GetBuffer();
					return true;
				}

				buffer = Array.Empty<byte>();
				return false;
			}

			// If no known field can be found, use a horribly slow try-catch instead.
			try
			{
				buffer = stream.GetBuffer();
				return true;
			}
			catch (UnauthorizedAccessException)
			{
				buffer = Array.Empty<byte>();
				return false;
			}
		}
	}
}
