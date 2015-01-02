/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
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
	/// Base parser to extend from
	/// </summary>
	public abstract class ParserBase
	{
		private const string TAG = "ParserBase";

		protected int ParseInt(string value)
		{
			int parsed;
			if (!int.TryParse(value, out parsed)) {
				Logger.Warn(TAG, "Unable to parse int: {0}", value);
			}
			return parsed;
		}

		protected float ParseFloat(string value)
		{
			float parsed;
			if (!float.TryParse(value, out parsed)) {
				Logger.Warn(TAG, "Unable to parse float: {0}", value);
			}
			return parsed;
		}

		protected List<float> ParseFloats(string value, string delimiter)
		{
			List<float> floats = new List<float>();
			foreach (string s in value.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)) {
				float parsed;
				if (float.TryParse(s, out parsed)) {
					floats.Add(parsed);
				} else {
					Logger.Warn(TAG, "Unable to parse float: {0}", s);
				}
			}
			return floats;
		}

		protected List<Pair<string, string>> ParsePairs(string value, string delimiter, string pairSeparator)
		{
			List<Pair<string, string>> pairs = new List<Pair<string, string>>();
			foreach (string s in value.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)) {
				string k, v;
				if (ParseSection(s, pairSeparator, out k, out v)) {
					pairs.Add(new Pair<string, string>(k, v));
				}
			}
			return pairs;
		}

		protected bool ParseSection(string s, string separator, out string key, out string value)
		{
			int index = s.IndexOf(separator);
			if (index > 0) {
				key = s.Substring(0, index);
				value = s.Substring(index + 1);
				return true;
			} else {
				key = null;
				value = null;
				Logger.Warn(TAG, "Unable to parse section: {0}", s);
				return false;
			}
		}

		protected string FindFile(string filename, string[] extensions, bool restrictExtensions, DirectoryInfo parentDirectory)
		{
			string path = FileLoader.FindFile(filename, extensions, restrictExtensions);
			if (string.IsNullOrEmpty(path)) {
				path = FileLoader.FindFile(Path.Combine(filename, parentDirectory.FullName), extensions, restrictExtensions);
			}
			return path;
		}
		
		protected string FindImage(string filename, DirectoryInfo parentDirectory)
		{
			return FindFile(filename, FileLoader.IMAGE_EXTENSIONS, false, parentDirectory);
		}
		
		protected string FindLyrics(string filename, DirectoryInfo parentDirectory)
		{
			return FindFile(filename, FileLoader.LYRICS_EXTENSIONS, false, parentDirectory);
		}
		
		protected string FindAudio(string filename, DirectoryInfo parentDirectory)
		{
			return FindFile(filename, FileLoader.AUDIO_EXTENSIONS, true, parentDirectory);
		}
	}
}
