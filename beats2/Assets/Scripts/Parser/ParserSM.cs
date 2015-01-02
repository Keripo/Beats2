/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;
using Beats2.Data;
using System.IO;
using System;

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
		private bool _isMetadataLoaded = false;

		public ParserSM(FileInfo inputFile, DirectoryInfo parentDirectory)
			: base(inputFile, parentDirectory)
		{
		}

		public override void LoadMetadata()
		{
			if (!_inputFile.Exists) {
				throw new ParserException(string.Format("Input file does not exist: {0}", _inputFile.FullName));
			}

			if (_isMetadataLoaded) {
				Logger.Warn(TAG, "Metadata was already previously loaded, reloading...");
			}

			try {
				using (FileStream stream = _inputFile.OpenRead()) {
					using (StreamReader reader = new StreamReader(stream)) {
						// Assume the input file size isn't larger than 2GB
						string buffer = reader.ReadToEnd();

						// If performance becomes an issue (due to memory copying),
						// replace String.Split with a String.indexOf implementation
						string[] sections = buffer.Split('#');
						foreach (string section in sections) {
							if (!string.IsNullOrEmpty(section)) {
								string sectionCleaned = section.Trim().TrimEnd(';');
								string tag, value;
								if (ParseSection(sectionCleaned, ":", out tag, out value) &&
									!string.IsNullOrEmpty(tag) &&
									!string.IsNullOrEmpty(value)) {
									ParseTag(tag, value);
								}
							}
						}
					}
				}

			} catch (Exception e) {
				throw new ParserException(string.Format("Failed to parse input file: {0}", _inputFile.FullName), e);
			}
		}

		public override void LoadCharts()
		{
			if (!_isMetadataLoaded) {
				throw new ParserException("Metadata must be loaded first");
			}

			// TODO: Load from _notesData, then add _events
		}
		
		private void ParseTag(string tag, string value)
		{
			switch (tag.ToUpper()) {
				case "VERSION":
					_simfile.metadata.simfileVersion = value;
					break;
				case "TITLE":
					_simfile.metadata.songTitle = value;
					break;
				case "TITLETRANSLIT":
					_simfile.metadata.songTitleTranslit = value;
					break;
				case "SUBTITLE":
					_simfile.metadata.songSubtitle = value;
					break;
				case "SUBTITLETRANSLIT":
					_simfile.metadata.songSubtitleTranslit = value;
					break;
				case "ARTIST":
					_simfile.metadata.songArtist = value;
					break;
				case "ARTISTTRANSLIT":
					_simfile.metadata.songArtistTranslit = value;
					break;
				case "GENRE":
					_simfile.metadata.songGenre = value;
					break;
				case "CREDIT":
					_simfile.metadata.infoCredits = value;
					break;
				case "BANNER":
					_simfile.metadata.graphicBanner = FindImage(value);
					break;
				case "BACKGROUND":
					_simfile.metadata.graphicBackground = FindImage(value);
					break;
				case "CDTITLE":
				case "JACKET":
				case "CDIMAGE":
				case "DISCIMAGE":
					if (_simfile.metadata.graphicCover == null) {
						_simfile.metadata.graphicCover = FindImage(value);
					}
					break;
				case "LYRICSPATH":
					ParseLyricsPath(value);
					break;
				case "MUSIC":
					_simfile.metadata.musicPath = FindAudio(value);
					break;
				case "OFFSET":
					_simfile.metadata.musicOffset = ParseFloat(value);
					break;
				case "SAMPLESTART":
					_simfile.metadata.musicSampleStart = ParseFloat(value);
					break;
				case "SAMPLELENGTH":
					_simfile.metadata.musicSampleLength = ParseFloat(value);
					break;
				case "DISPLAYBPM":
					_simfile.metadata.musicDisplayBpm = ParseFloats(value, ":");
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

		private void ParseLyricsPath(string value)
		{
			string lyricPath = FindLyrics(value);
			if (string.IsNullOrEmpty(lyricPath)) {
				Logger.Warn(TAG, "Unable to find lyrics file: {0}", lyricPath);
			} else {
				Lyrics lyrics = new Lyrics();
				lyrics.filePath = lyricPath;
				_simfile.lyrics.Add(lyrics);
			}
		}

		private void ParseBpms(string value)
		{
			foreach (Pair<string, string> pair in ParsePairs(value, ",", "=")) {
				int beat = ParseInt(pair.key);
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
				int beat = ParseInt(pair.key);
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
				int beat = ParseInt(pair.key);
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
				int beat = ParseInt(pair.key);
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
