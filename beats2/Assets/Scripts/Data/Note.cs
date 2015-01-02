/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;

namespace Beats2.Data
{

	public class Note
	{
		public NoteState state;
		public NoteType type;
		public int fraction;
		public List<NotePoint> points = new List<NotePoint>();
	}
    
	public enum NoteState
	{
		Inactive,
		Active,
		Started,
		Focused,
		Completed,
		Missed
	}

	// Note:
	// Beats2 is touch-based rhythm game, not an arcade machine simulator.
	// The following list of tags are not supported or remaped:
	// From StepMania 4.0's .SM format:
	// * Lift -> Hold
	// * Shock -> Mine
	// Also note that certain notetypes may be treated as other types
	// depending on the gameplay style
	public enum NoteType
	{
		Tap,
		Hold, // Freeze
		Slide, // Drag
		Roll,
		Repeat,
		Chain,
		Mine,
		Fake,
		Spinner
	}
    
	public struct NotePoint
	{
		public int x;
		public int y;
		public bool coord;

		public float beat;
		public float time;

		public bool hit;
	}
}
