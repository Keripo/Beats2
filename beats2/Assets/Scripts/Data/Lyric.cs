/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;

namespace Beats2.Data
{
	public class Lyric
	{
		public string language;
		public string filePath;
		public List<LyricLine> lines = new List<LyricLine>();
	}

	public struct LyricLine
	{
		public int colour;
		public double time;
		public string line;
	}
}