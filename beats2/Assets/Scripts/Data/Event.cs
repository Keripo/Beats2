/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

namespace Beats2.Data
{
	public class Event
	{
		public EventType type;
		public float beat;
		public float time;
		public object value;
	}

	// Note:
	// Beats2 is touch-based rhythm game, not an arcade machine simulator.
	// The following list of tags are not supported or remaped:
	// From StepMania 5.0's .SSC format:
	// * Delays -> Stop
	// * Warps -> ignored
	// * Fakes -> ignored
	// * Speeds -> ignored
	// * Scrolls -> built-in
	// * TimeSignatures -> ignored
	// * TickCounts -> ignored
	// * Combos -> ignored
	public enum EventType
	{
		Bpm,
		Stop,
		Label,
		BgChange
	}
}
