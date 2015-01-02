/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;
using Beats2.Data;
using System;

namespace Beats2.Parser
{
	
	/// <summary>
	/// Base parser to extend from
	/// </summary>
	public abstract class ParserBase
	{
		private const string TAG = "ParserBase";
		
		public Simfile simfile;

		protected string _simfilePath;
		protected string _parentFolderPath;
		protected string _rawData;

		public static ParserBase GetParser(string simfilePath)
		{
			if (!FileLoader.FileExists(simfilePath)) {
				throw new ParserException(string.Format("Simfile does not exist: {0}", simfilePath));
			}

			string extension = FileLoader.GetFileExtension(simfilePath).ToLower();
			switch (extension) {
				case ".sm":
				case ".ssc":
					return new ParserSM(simfilePath);
				default:
					throw new ParserException(string.Format("Unable to find parser for format: {0}", extension));
			}
		}

		protected ParserBase(string simfilePath)
		{
			_simfilePath = simfilePath;
			_parentFolderPath = FileLoader.GetParentFolder(simfilePath);
			simfile = new Simfile();
		}

		public string LoadData(bool reload = false)
		{
			if (_rawData == null || reload) {
				_rawData = FileLoader.LoadText(_simfilePath);
				if (string.IsNullOrEmpty(_rawData)) {
					throw new ParserException("Failed to load raw data");
				}
			}
			return _rawData;
		}

		public abstract void LoadMetadata();

		public abstract void LoadLyrics();

		public abstract void LoadCharts();

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

		protected string FindFile(string filename, string[] extensions, bool restrictExtensions)
		{
			if (string.IsNullOrEmpty(filename)) {
				return null;
			}

			string path = FileLoader.FindFile(
				FileLoader.GetPath(filename, _parentFolderPath), extensions, restrictExtensions);
			if (string.IsNullOrEmpty(path)) {
				// Most simfiles use relative paths, but check absolute in case
				path = FileLoader.FindFile(filename, extensions, restrictExtensions);
			}
			return path;
		}
		
		protected string FindImage(string filename)
		{
			return FindFile(filename, FileLoader.IMAGE_EXTENSIONS, false);
		}
		
		protected string FindLyrics(string filename)
		{
			return FindFile(filename, FileLoader.LYRICS_EXTENSIONS, false);
		}
		
		protected string FindAudio(string filename)
		{
			return FindFile(filename, FileLoader.AUDIO_EXTENSIONS, true);
		}
	}
}
