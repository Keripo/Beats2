/*
	Copyright (c) 2013, Keripo
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
	    * Redistributions of source code must retain the above copyright
	      notice, this list of conditions and the following disclaimer.
	    * Redistributions in binary form must reproduce the above copyright
	      notice, this list of conditions and the following disclaimer in the
	      documentation and/or other materials provided with the distribution.
	    * Neither the name of the <organization> nor the
	      names of its contributors may be used to endorse or promote products
	      derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace Beats2.Data {

	/// <summary>
	/// Parser for .bsf files (Beats Simfiles)
	/// </summary>
	public class ParserBSF : ParserBase {
		new protected const string TAG = "ParserBSF";

		/// <summary>
		/// "Info" section header name
		/// </summary>
		private const string INFO_SECTION = "Info";

		/// <summary>
		/// Minimum .bsf format version supported by this parser
		/// </summary>
		private const int MIN_VERSION = 1;

		/// <summary>
		/// Checks the Beats simfile format version
		/// </summary>
		private void CheckVersion(string val) {
			int version = this.ParseInt("Version", val, true);
			if (version < MIN_VERSION) {
				this.Error("Format version of " + version + " is less than the minimum supported version " + MIN_VERSION);
			}
		}

		/// <summary>
		/// Parses a key-value pair using '=' as the separator, see <see cref="ParserBase.ParseKeyValuePair"/>
		/// </summary>
		private bool ParseKeyValuePair(string line, out string key, out string val) {
			return this.ParseKeyValuePair(line, '=', out key, out val);
		}

		/// <summary>
		/// Parses for DisplayBPM values
		/// </summary>
		private List<float> ParseDisplayBpms(string val) {
			List<float> bpms = new List<float>();
			foreach (string s in val.Split(',')) {
				if (s == "*") {
					bpms.Add(Info.RANDOM_DISPLAY_BPM);
				} else {
					float bpm = this.ParseFloat("DisplayBpm", s, false);
					if (bpm > 0f) {
						bpms.Add(bpm);
					}
				}
			}
			return bpms;
		}

		/// <summary>
		/// Parses for Backgrounds image files
		/// </summary>
		private List<string> ParseBackgrounds(string val) {
			List<string> backgrounds = new List<string>();
			foreach (string s in val.Split(',')) {
				string background = this.FindImageFile("Background", s, false);
				if (background != null) {
					backgrounds.Add(background);
				}
			}
			return backgrounds;
		}

		/// <summary>
		/// Parses a pattern section
		/// </summary>
		private Pattern ParsePattern(string section) {
			// Indicies for easy referene
			int indexDash = section.IndexOf('-');
			int indexComma = section.IndexOf(',');

			// Parse pattern type
			PatternType patternType = PatternType.BEATS;
			try {
				patternType = (PatternType)Enum.Parse(
					typeof(PatternType),
					section.Substring(0, indexDash),
					true
					);
			} catch (ArgumentException e) {
				this.Error("Unable to parse section game type, " + e.Message);
			}

			// Parse key count
			int keyCount;
			if (!int.TryParse(section.Substring(indexDash + 1, (indexComma - 1) - indexDash), out keyCount)) {
				this.Error("Unable to parse sectionkey count");
			}

			// Parse difficulty level
			int difficulty;
			if (!int.TryParse(section.Substring(indexComma + 1), out difficulty)) {
				this.Error("Unable to parse section difficulty level");
			}

			// Set the pattern
			Pattern pattern = new Pattern();
			pattern.type = patternType;
			pattern.keyCount = keyCount;
			pattern.difficulty = difficulty;
			pattern.lineIndex = _index + 1;
			pattern.loaded = false;
			return pattern;
		}

		public override void Load(string path) {
			base.Load(path);

			// Parse line-by-line
			_index = 0;
			_line = "";
			string section = String.Empty;
			ReadAheadStreamReader reader = new ReadAheadStreamReader(path);

			// Main loop
			while (reader.PeekLine() != null) {
				_index++;
				_line = reader.ReadLine().Trim();

				// Empty line
				if (_line.Length == 0) {
					continue;
				// Comment
				} else if (_line[0] == '/' || _line[0] == ';') {
					continue;
				// Section start
				} else if (_line[0] == '[') {
					if (_line.IndexOf(']') != -1) {
						section = _line.Substring(_line.IndexOf('[') + 1, _line.IndexOf(']') - 1);
						if (section == INFO_SECTION) {
							continue;
						} else if (section.IndexOf('-') != -1 && section.IndexOf(',') != -1) {
							Pattern pattern = this.ParsePattern(section);
							_info.patterns.Add(pattern);
						}
					}
				// Info section
				// Note: pattern data is parsed with LoadPattern
				} else if (section == INFO_SECTION) {
					// Parse key-value pair
					string key, val;
					if (!this.ParseKeyValuePair(_line, out key, out val)) {
						// Skip if not a key-value pair
						this.Warning("Unparsed line");
						continue;
					}
					// Set data based on key
					switch(key) {
						case "Version":				this.CheckVersion(val);												break;
						case "Song":				_info.song = this.FindAudioFile("Song", val, true);					break;
						case "Title":				_info.title = val; 													break;
						case "TitleTranslit":		_info.titleTranslit = val;											break;
						case "Subtitle":			_info.subtitle = val; 												break;
						case "SubtitleTranslit":	_info.subtitleTranslit = val;										break;
						case "Artist":				_info.artist = val;													break;
						case "ArtistTranslit":		_info.artistTranslit = val;											break;
						case "Album":				_info.album = val;													break;
						case "AlbumTranslit":		_info.albumTranslit = val;											break;
						case "Genre":				_info.genre = val;													break;
						case "Credits":				_info.credits = val;												break;
						case "Link":				_info.link = val;													break;
						case "Description":			_info.description = val;											break;
						case "Tags":				_info.tags.AddRange(val.Split(','));								break;
						case "DisplayBpm":			_info.displayBpm = this.ParseDisplayBpms(val);						break;
						case "SampleStart":			_info.sampleStart = this.ParseFloat("SampleStart", val, false);		break;
						case "SampleLength":		_info.sampleLength = this.ParseFloat("SampleLength", val, false);	break;
						case "Cover":				_info.cover = this.FindImageFile("Cover", val, false);				break;
						case "Banner":				_info.banner = this.FindImageFile("Banner", val, false);			break;
						case "Backgrounds":			_info.backgrounds = this.ParseBackgrounds(val);						break;
						case "Lyrics":				_info.lyrics = this.FindLyricsFile("Lyrics", val, false);			break;
						default:					this.Warning("Unrecognized key: " + key);							break;
					}
				} else {
					this.Warning("Unparsed line");
				}
			}

			// Cleanup
			reader.Close();
		}

		/// <summary>
		/// Parses the NoteType
		/// </summary>
		private NoteType ParseNoteType(char c) {
			switch (c) {
				case '0': return NoteType.MINE;
				case '1': return NoteType.TAP;
				case '2': return NoteType.HOLD;
				case '3': return NoteType.ROLL;
				case '4': return NoteType.REPEAT;
				case '5': return NoteType.SLIDE;
				case 'L': return NoteType.LABEL;
				case 'G': return NoteType.BG;
				case 'B': return NoteType.BPM;
				case 'S': return NoteType.STOP;
				default: return NoteType.UNKNOWN;
			}
		}

		/// <summary>
		/// Parses notes data
		/// </summary>
		private void ParseNote(Pattern pattern, string noteType, string noteData) {
			Note note;
			string[] commaSplit;
			string[] lineSplit;
			string[] colonSplit;
			float time;
			int column;

			// For now, note types are single characters, may change if necessary in future
			if (noteType.Length != 1) {
				this.Warning("Unrecognized note type");
				return;
			}

			// Get the first time, because its used by everyone
			colonSplit = noteData.Split(':');
			if (colonSplit.Length < 2) goto ParseWarning;
			time = this.ParseFloat("note time", colonSplit[0], false);
			if (time == -1f) goto ParseWarning;

			// Parse based on note type
			NoteType type = this.ParseNoteType(noteType[0]);
			switch (type) {
				// MINE:   0=time:column,column,column,...
				case NoteType.MINE:
				// TAP:    1=time:column,column,column,...
				case NoteType.TAP:
					// Note columns
					commaSplit = noteData.Split(',');
					foreach (string s in commaSplit) {
						column = this.ParseInt(type + " note column", s, false);
						if (column == -1) goto ParseWarning;
						note = new Note();
						note.type = type;
						note.AddPoint(time, column);
						this.AddNote(pattern, note);
					}
					break;
				// HOLD:   2=time:column,column,column,...
				case NoteType.HOLD:
				// ROLL:   3=time:column,column,column,...|time
				case NoteType.ROLL:
					// Note end time
					lineSplit = colonSplit[1].Split('|');
					if (lineSplit.Length != 2) goto ParseWarning;
					float endTime = this.ParseFloat(type + " note end time", lineSplit[1], false);
					if (endTime == -1f) goto ParseWarning;

					// Columns
					commaSplit = colonSplit[0].Split(',');
					foreach (string s in commaSplit) {
						column = this.ParseInt(type + " note column", s, false);
						if (column == -1) goto ParseWarning;
						note = new Note();
						note.type = type;
						note.AddPoint(time, column);
						note.AddPoint(endTime, column);
						this.AddNote(pattern, note);
					}
					break;
				// REPEAT: 4=time:column,column,column,...|time|time|time|...
				case NoteType.REPEAT:
					List<float> times = new List<float>();
					List<int> columns = new List<int>();

					// Note start time
					times.Add(time);

					// Time split
					lineSplit = noteData.Split('|');
					if (lineSplit.Length < 2) goto ParseWarning;
					for (int i = 1; i < lineSplit.Length; i++) {
						time = this.ParseFloat(type + " note time", lineSplit[i], false);
						if (time == -1f) goto ParseWarning;
						times.Add(time);
					}

					// Column
					commaSplit = lineSplit[0].Split(',');
					if (commaSplit.Length < 1) goto ParseWarning;
					foreach (string s in commaSplit) {
						column = this.ParseInt(type + " note column", s, false);
						if (column == -1) goto ParseWarning;
						columns.Add(column);
					}

					// Add notes
					foreach (int c in columns) {
						note = new Note();
						note.type = type;
						foreach (int t in times) {
							note.AddPoint(t, c);
						}
						this.AddNote(pattern, note);
					}
					break;
				// SLIDE:  5=time:column|time:column|time:column|...
				case NoteType.SLIDE:
					// Only one Note per Slide line
					note = new Note();
					note.type = type;

					// Split into time:column pairs
					lineSplit = noteData.Split('|');
					if (lineSplit.Length < 2) goto ParseWarning;

					// Parse each pair
					foreach (string s in lineSplit) {
						colonSplit = s.Split(':');
						if (colonSplit.Length != 2) goto ParseWarning;
						time = this.ParseFloat(type + " note time", colonSplit[0], false);
						if (time == -1f) goto ParseWarning;
						column = this.ParseInt(type + " note column", colonSplit[1], false);
						if (column == -1f) goto ParseWarning;
						note.AddPoint(time, column);
					}

					// Add the note
					this.AddNote(pattern, note);
					break;
				// LABEL:  L=time:text
				case NoteType.LABEL:
					string text = colonSplit[1];
					note = new Note();
					note.type = NoteType.LABEL;
					note.eventTime = time;
					note.eventStringVal = text;
					this.AddNote(pattern, note);
					break;
				// BG:     G=time:index
				case NoteType.BG:
					int backgroundIndex = this.ParseInt("Background index", colonSplit[1], false);
					if (backgroundIndex == -1) goto ParseWarning;
					note = new Note();
					note.type = NoteType.BG;
					note.eventTime = time;
					note.eventIntVal = backgroundIndex;
					this.AddNote(pattern, note);
					break;
				// BPM:    B=time:value
				case NoteType.BPM:
					float bpm = this.ParseFloat("BPM change value", colonSplit[1], false);
					if (bpm == -1f) goto ParseWarning;
					note = new Note();
					note.type = NoteType.BPM;
					note.eventTime = time;
					note.eventFloatVal = bpm;
					this.AddNote(pattern, note);
					break;
				// STOP:   S=time:duration
				case NoteType.STOP:
					float duration = this.ParseFloat("Stop duration value", colonSplit[1], false);
					if (duration == -1f) goto ParseWarning;
					note = new Note();
					note.type = NoteType.STOP;
					note.eventTime = time;
					note.eventFloatVal = duration;
					this.AddNote(pattern, note);
					break;
				default:
					goto ParseWarning;
			}

			// Generic warning, out of laziness
			ParseWarning:
				Warning("Improperly formatted line");
		}

		public override void LoadPattern(Pattern pattern) {
			// Don't unnecessarily reload
			if (pattern.loaded == true) {
				return;
			}

			// Parse line-by-line
			_index = 0;
			_line = "";
			ReadAheadStreamReader reader = new ReadAheadStreamReader(_info.path);

			// Skip to main section
			while (_index < pattern.lineIndex) {
				_index++;
				reader.SkipLine();
			}

			// Parsing loop
			while (reader.PeekLine() != null) {
				_index++;
				_line = reader.ReadLine().Trim();

				// Empty line
				if (_line.Length == 0) {
					continue;
				// Comment
				} else if (_line[0] == ';' || _line[0] == '/') {
					continue;
				// Section start
				} else if (_line[0] == '[') {
					break; // End of pattern data
				// Pattern data
				} else {
					// Parse key-value pair
					string key, val;
					if (!this.ParseKeyValuePair(_line, out key, out val)) {
						// Skip if not a key-value pair
						this.Warning("Unparsed line");
						continue;
					}
					this.ParseNote(pattern, key, val);
				}
			}

			// Sort notes
			this.SortNotes(pattern);

			// Cleanup
			pattern.loaded = true;
			reader.Close();
		}
	}
}

