using System;
using System.IO;
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.Data {

	/// <summary>
	/// Simple INI file parser
	/// </summary>
	public class SettingsFile {
		private const string TAG = "IniFile";
		private const string COMMENT_CHARS = ";#/";
		private Dictionary<string, Dictionary<string, string>> _content;

		public SettingsFile(string path) {
			_content = new Dictionary<string, Dictionary<string, string>>();
			Parse(path);
		}

		private void Parse(string path) {
			StreamReader reader = new StreamReader(path);

			string line;
			string sectionName = String.Empty;
			Dictionary<string, string> sectionValues = new Dictionary<string, string>();

			while ((line = reader.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length == 0) { // Empty line
					continue;
				} else if (COMMENT_CHARS.IndexOf(line[0]) != -1) { // Comment
					continue;
				} else if (line[0] == '[') { // Section start
					if (line.IndexOf(']') != -1) {
						sectionName = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - 1);
						if (!_content.ContainsKey(sectionName)) {
							sectionValues = new Dictionary<string, string>();
							_content.Add(sectionName, sectionValues);
						}
					}
				} else { // Key-value pair
					if (line.IndexOf('=') != -1) {
						int indexEquals = line.IndexOf('=');
						string key = line.Substring(0, indexEquals);
						string val = line.Substring(indexEquals + 1);
						if (!_content[sectionName].ContainsKey(key)) {
							_content[sectionName][key] = val;
						} else {
							_content[sectionName].Add(key, val);
						}
					}
				}
			}
			reader.Close();
		}

		public string Get(string section, string key) {
			if (_content.ContainsKey(section) && _content[section].ContainsKey(key)) {
				return _content[section][key];
			} else {
				Logger.Error(TAG, String.Format("Unable to fetch key \"{0}\" from section \"{1}\"", key, section));
				return null;
			}
		}

		public bool Set(string section, string key, string val) {
			if (_content.ContainsKey(section) && _content[section].ContainsKey(key)) {
				_content[section][key] = val;
				return true;
			} else {
				Logger.Error(TAG, String.Format("IniFile does not contain key \"{0}\" from section \"{1}\"", key, section));
				return false;
			}
		}

		public bool Write(string url) {
			StreamWriter writer;
			if (File.Exists(url)) {
				writer = new StreamWriter(url, false);
			} else {
				writer = File.CreateText(url);
			}
			string line;
			foreach (KeyValuePair<string, Dictionary<string, string>> section in _content) {
				line = String.Format("[{0}]", section.Key);
				writer.WriteLine(line);
				foreach (KeyValuePair<string, string> pair in section.Value) {
					line = String.Format("{0}={1}", pair.Key, pair.Value);
					writer.WriteLine(line);
				}
			}
			writer.Flush();
			writer.Close();
			return true;
		}
	}
}
