/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

namespace Beats2.Parser
{

	public class ParserTest : MonoBehaviour
	{
		private const string TAG = "ParserTest";
		
		public string simfilePath;
		public Text contentText;

		public Text logHistoryText;
		public int logHistorySize;

		private ParserBase _parser;

		void Awake()
		{
			Logger.SetLogHistory(true);
			Logger.SetLogHistorySize(logHistorySize);
		}
		
		void Start()
		{
			_parser = ParserBase.GetParser(FileLoader.GetDataPath(simfilePath));
		}
		
		void Update()
		{
			logHistoryText.text = Logger.GetLogHistoryString();
		}

		public void LoadMetadata()
		{
			Logger.Debug(TAG, "LoadMetadata");
			_parser.LoadMetadata();
			contentText.text = Logger.DumpFields(_parser.simfile.metadata);
		}

		public void LoadLyrics()
		{
			Logger.Debug(TAG, "LoadLyrics");
			_parser.LoadLyrics();
			contentText.text = Logger.DumpFields(_parser.simfile.lyrics);
		}

		public void LoadCharts()
		{
			Logger.Debug(TAG, "LoadCharts");
			_parser.LoadCharts();
			contentText.text = Logger.DumpFields(_parser.simfile.charts);
		}
	}
}

