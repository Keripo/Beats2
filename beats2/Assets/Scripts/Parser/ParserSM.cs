/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;
using Beats2.Data;
using System;
using System.IO;
using System.Text;

namespace Beats2.Parser
{

	/// <summary>
	/// Parser for .sm/.ssc files (StepMania)
	/// <see cref="http://www.stepmania.com/wiki/file-formats/"/>
	/// </summary>
	public class ParserSM : ParserBase
	{
		private const string TAG = "ParserSM";

		private List<Event> _events = new List<Event>();
		private List<string> _notesData = new List<string>();
		private string _lyricsPath = null;
		private bool _isMetadataLoaded = false;

		public ParserSM(string simfilePath) : base(simfilePath)
		{
		}

		public override void LoadMetadata()
		{
			if (_isMetadataLoaded) {
				Logger.Warn(TAG, "Metadata was already previously loaded, reloading...");
			}

			foreach (string section in LoadSections()) {
				string tag, value;
				if (ParseSection(section, ":", out tag, out value) &&
					!string.IsNullOrEmpty(tag) &&
					!string.IsNullOrEmpty(value)) {
					ParseTag(tag, value);
				}
			}

			_isMetadataLoaded = true;
		}

		public override void LoadLyrics()
		{
			if (!_isMetadataLoaded) {
				throw new ParserException("Metadata must be loaded first before lyrics");
			}

			// TODO: Load from _lyricsPath
		}

		public override void LoadCharts()
		{
			if (!_isMetadataLoaded) {
				throw new ParserException("Metadata must be loaded first before charts");
			}

			// TODO: Load from _notesData, then add _events
		}
		
		private List<string> LoadSections()
		{
			List<string> sections = new List<string>();
			StringBuilder buffer = new StringBuilder();

			using (StringReader reader = new StringReader(LoadData())) {
				// Remove all comment lines and empty lines
				String line;
				while ((line = reader.ReadLine()) != null) {
					line = line.Trim();
					if (line.Length > 0 && !line.StartsWith("//")) {
						buffer.Append(line);
					}
				}
				
				// If performance becomes an issue (due to memory copying),
				// replace String.Split with a String.indexOf implementation
				string[] sectionsRaw = buffer.ToString().Split('#');
				foreach (string sectionRaw in sectionsRaw) {
					if (!string.IsNullOrEmpty(sectionRaw)) {
						string section = sectionRaw.Trim();
						int indexEnd = section.IndexOf(';');
						if (indexEnd > 0) {
							section = section.Substring(0, indexEnd);
						}
						if (section.Length > 0) {
							sections.Add(section);
						}
					}
				}
			}
			
			return sections;
		}
		
		private void ParseTag(string tag, string value)
		{
			switch (tag.ToUpper()) {
				case "VERSION":
					simfile.metadata.simfileVersion = value;
					break;
				case "TITLE":
					simfile.metadata.songTitle = value;
					break;
				case "TITLETRANSLIT":
					simfile.metadata.songTitleTranslit = value;
					break;
				case "SUBTITLE":
					simfile.metadata.songSubtitle = value;
					break;
				case "SUBTITLETRANSLIT":
					simfile.metadata.songSubtitleTranslit = value;
					break;
				case "ARTIST":
					simfile.metadata.songArtist = value;
					break;
				case "ARTISTTRANSLIT":
					simfile.metadata.songArtistTranslit = value;
					break;
				case "GENRE":
					simfile.metadata.songGenre = value;
					break;
				case "CREDIT":
					simfile.metadata.infoCredits = value;
					break;
				case "BANNER":
					simfile.metadata.graphicBanner = FindImage(value);
					break;
				case "BACKGROUND":
					simfile.metadata.graphicBackground = FindImage(value);
					break;
				case "CDTITLE":
				case "JACKET":
				case "CDIMAGE":
				case "DISCIMAGE":
					if (simfile.metadata.graphicCover == null) {
						simfile.metadata.graphicCover = FindImage(value);
					}
					break;
				case "LYRICSPATH":
					_lyricsPath = FindLyrics(value);
					break;
				case "MUSIC":
					simfile.metadata.musicPath = FindAudio(value);
					break;
				case "OFFSET":
					simfile.metadata.musicOffset = ParseFloat(value);
					break;
				case "SAMPLESTART":
					simfile.metadata.musicSampleStart = ParseFloat(value);
					break;
				case "SAMPLELENGTH":
					simfile.metadata.musicSampleLength = ParseFloat(value);
					break;
				case "DISPLAYBPM":
					simfile.metadata.musicDisplayBpm = ParseFloats(value, ":");
					break;
				case "BPMS":
					ParseBpms(value);
					break;
				case "STOPS":
				case "DELAYS":
				case "FREEZES":
					// Note: Stops and delays are treated the same here because
					// I'm too lazy to support both mechanics
					ParseStops(value);
					break;
				case "BGCHANGES":
				case "ANIMATIONS":
					ParseBgChanges(value);
					break;
				case "LABELS":
					ParseLabels(value);
					break;
				case "ATTACKS":
				case "WARPS":
				case "COMBOS":
				case "SPEEDS":
				case "SCROLLS":
				case "FAKES":
					// I have no plans on supporting these whacky mechanics
					// Feel free to submit a patch if you really want them
					break;
				case "FGCHANGES":
				case "SELECTABLE":
				case "MENUCOLOR":
				case "TIMESIGNATURES":
				case "TICKCOUNTS":
				case "INSTRUMENTTRACK":
				case "KEYSOUNDS":
				case "ORIGIN":
				case "MUSICLENGTH":
				case "LASTBEATHINT":
				case "LASTSECONDHINT":
				case "MUSICBYTES":
					// Currently unsupported, may add support later
					break;
				case "NOTES":
				case "NOTES2":
					// These will be parsed separately by LoadCharts()
					_notesData.Add(value);
					break;
				default:
					Logger.Warn(TAG, "Unsupported tag: {0}:{1}", tag, value);
					break;
			}
		}

		private void ParseBpms(string value)
		{
			foreach (Pair<string, string> pair in ParsePairs(value, ",", "=")) {
				float beat = ParseFloat(pair.key);
				float bpm = ParseFloat(pair.value);
				if (beat < 0f) {
					Logger.Warn(TAG, "Negative beat value events ignored");
				} else if (bpm <= 0f) {
					Logger.Warn(TAG, "Zero or negative BPMs not supported");
				} else {
					Event ev = new Event();
					ev.type = EventType.Bpm;
					ev.beat = beat;
					ev.value = bpm;
					_events.Add(ev);
				}
			}
		}

		private void ParseStops(string value)
		{
			foreach (Pair<string, string> pair in ParsePairs(value, ",", "=")) {
				float beat = ParseFloat(pair.key);
				float stop = ParseFloat(pair.value);
				if (beat < 0f) {
					Logger.Warn(TAG, "Negative beat value events ignored");
				} else if (stop <= 0f) {
					Logger.Warn(TAG, "Zero or negative stops not supported");
				} else {
					Event ev = new Event();
					ev.type = EventType.Stop;
					ev.beat = beat;
					ev.value = stop;
					_events.Add(ev);
				}
			}
		}

		private void ParseBgChanges(string value)
		{
			foreach (Pair<string, string> pair in ParsePairs(value, ",", "=")) {
				float beat = ParseFloat(pair.key);
				string filename = pair.value;
				if (filename.Contains("=")) {
					// No plans on supporting all those fancy transition flages
					// See http://www.stepmania.com/forums/general-stepmania/show/1393#post3757
					filename = filename.Substring(0, filename.IndexOf('='));
				}
				string filePath = FindImage(filename);
				if (beat < 0f) {
					Logger.Warn(TAG, "Negative beat value events ignored");
				} else if (string.IsNullOrEmpty(filename)) {
					Logger.Warn(TAG, "Unable to find background image: {0}", filename);
				} else {
					Event ev = new Event();
					ev.type = EventType.BgChange;
					ev.beat = beat;
					ev.value = filePath;
					_events.Add(ev);
				}
			}
		}

		private void ParseLabels(string value)
		{
			foreach (Pair<string, string> pair in ParsePairs(value, ",", "=")) {
				float beat = ParseFloat(pair.key);
				if (beat < 0f) {
					Logger.Warn(TAG, "Negative beat value events ignored");
				} else {
					Event ev = new Event();
					ev.type = EventType.Label;
					ev.beat = beat;
					ev.value = pair.value;
					_events.Add(ev);
				}
			}
		}
	}
}
