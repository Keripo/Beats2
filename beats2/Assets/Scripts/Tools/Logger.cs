/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Beats2
{

	public static class Logger
	{
		private static Queue<LogEntry> _history = new Queue<LogEntry>();
		private static int _historyMaxSize = 100;
		private static bool _debug = true;
		
		public static void SetDebug(bool debug)
		{
			_debug = debug;
		}
        
		public static void Debug(string tag, string format, params object[] args)
		{
			if (_debug) {
				string msg = FormatLogString("D", tag, format, args);
				UnityEngine.Debug.Log(msg);
			}
		}
        
		public static void Log(string tag, string format, params object[] args)
		{
			string msg = FormatLogString("L", tag, format, args);
			UnityEngine.Debug.Log(msg);
		}
        
		public static void Warn(string tag, string format, params object[] args)
		{
			string msg = FormatLogString("W", tag, format, args);
			UnityEngine.Debug.LogWarning(msg);
		}
        
		public static void Error(string tag, string format, params object[] args)
		{
			string msg = FormatLogString("E", tag, format, args);
			UnityEngine.Debug.LogError(msg);
		}
        
		public static void Error(string tag, Exception e, string format, params object[] args)
		{
			string msg = FormatLogString("E", tag, format, args);
			UnityEngine.Debug.LogError(msg);
			UnityEngine.Debug.LogException(e);
		}
		
		public static void SetLogHistory(bool enabled)
		{
			if (enabled) {
				_history = new Queue<LogEntry>();
				Application.RegisterLogCallback(SaveLogHistory);
			} else {
				Application.RegisterLogCallback(null);
			}
		}
		
		public static void SetLogHistorySize(int size)
		{
			_historyMaxSize = size;
		}
        
		public static Logger.LogEntry[] GetLogHistory()
		{
			return _history.ToArray();
		}
		
		public static string GetLogHistoryString()
		{
			StringBuilder logHistory = new StringBuilder();
			foreach (LogEntry logEntry in _history.ToArray()) {
				logHistory.AppendLine(logEntry.ToString().Trim());
			}
			return logHistory.ToString();
		}

		public static string DumpFields(object obj, int indentCount = 0)
		{
			string indent = new string('\t', indentCount);
			string indentInner = new string('\t', indentCount + 1);
			StringBuilder properties = new StringBuilder();

			properties.AppendLine(indent + "{");

			// Use reflection to get list of properties and their values
			Type type = obj.GetType();
			foreach (FieldInfo field in type.GetFields()) {
				string name = field.Name;
				object value = field.GetValue(obj);
				if (value is ICollection) {
					// Recursively print inner objects
					properties.AppendLine(string.Format("{0}{1} =", indentInner, name));
					foreach (ICollection item in ((ICollection)value)) {
						properties.AppendLine(DumpFields(item, indentCount + 1));
					}
				} else {
					properties.AppendLine(string.Format("{0}{1} = {2}", indentInner, name, value));
				}
			}
			properties.AppendLine(indent + "}");

			return properties.ToString();
		}

		private static string FormatLogString(string type, string tag, string format, params object[] args)
		{
			return String.Format("{0}|{1}: {2}", type, tag, String.Format(format, args));
		}
		
		private static void SaveLogHistory(string logString, string stackTrace, LogType type)
		{
			LogEntry logEntry = new LogEntry();
			logEntry.type = type;
			logEntry.logString = logString;
			logEntry.stackTrace = stackTrace;
			logEntry.time = DateTime.Now;
			while (_history.Count >= _historyMaxSize) {
				_history.Dequeue();
			}
			_history.Enqueue(logEntry);
		}
		
		public class LogEntry
		{
			public LogType type;
			public string logString;
			public string stackTrace;
			public DateTime time;

			public override string ToString()
			{
				return string.Format(
					"{0}: {1}",
					time.ToLongTimeString(),
					type.Equals(LogType.Exception) ? stackTrace : logString
				);
			}
		}
	}
}