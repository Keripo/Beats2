/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

namespace Beats2.Data
{
	public class ChartInfo
	{
		public ChartStyle style;
		public ChartType type;
		public int keyCount;

		public ChartDifficulty difficulty;
		public string difficultyName;
		public int difficultyValue;
		public ChartRadar radar;

		public string credits;
		public string description;
	}

	public enum ChartStyle
	{
		Beats,
		Technika,
		Square,
		Taiko,
		Mai,
		Diva,
		Osu
	}

	public enum ChartType
	{
		Pad,
		Keyboard,
		Touch,
		Kinect,
		Unknown
	}

	// See https://github.com/stepmania/stepmania/blob/master/src/Difficulty.cpp
	public enum ChartDifficulty
	{
		Tutorial,
		Beginner,
		Easy, // Basic, Light
		Medium, // Another, Trick, Standard, Difficult
		Hard, // SSR, Maniac, Heavy
		Challenge, // SManiac, Expert, Oni
		Edit,
		Unknown
	}

	// See http://dancedancerevolutionddr.wikia.com/wiki/Groove_Radar
	public struct ChartRadar
	{
		public int stream;
		public int voltage;
		public int air;
		public int freeze;
		public int chaos;
	}

}
