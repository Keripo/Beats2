using UnityEngine;
using System;
using System.IO;
using Beats2;

namespace Beats2.System {

	public static class SysPath {
		private const string TAG = "SysPath";
		private const string BEATS2_DIR = "Beats2";
		private const string FILE_PROTOCOL = "file://";

		public static string[] AudioExtensions = {
			// TODO: Temporary fix til I use an FMOD plugin for LoadAudio
#if UNITY_ANDROID
			".mp3",
#else
			".ogg",
#endif
			".wav"
		};

		public static string[] ImageExtensions = {
			".png",
			".jpg",
			".bmp"
		};

		public static string[] LyricsExtensions = {
			".lrc"
		};

		public static string dataPath;

		public static void Init() {
			SetDataPath();

			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			Logger.Debug(TAG, "Reset...");
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		private static string[] sdcardPaths = {
			"/mnt/sdcard/external_sd",
			"/mnt/sdcard/sd",
			"/sdcard/external_sd",
			"/sdcard/sd",
			"/mnt/external_sd",
			"/mnt/sdcard-ext",
			"/mnt/sdcard",
			"/mnt/sd",
			"/mnt/external_sd",
			"/sdcard-ext",
			"/sdcard",
			"/sd"
		};
#endif

		private static void SetDataPath() {

			// Find an existing Beats2 directory
#if UNITY_ANDROID && !UNITY_EDITOR
			foreach (string path in sdcardPaths) {
				dataPath = Path.Combine(path, BEATS2_DIR);
				if (Directory.Exists(dataPath)) {
					return;
				}
			}
#else
			string path = Application.dataPath;
			DirectoryInfo dir = new DirectoryInfo(path);
			if (dir.Parent != null) {
				path = dir.Parent.FullName;
			}
			dataPath = Path.Combine(path, BEATS2_DIR);
			if (Directory.Exists(dataPath)) {
				return;
			}
#endif
			// TODO - replace this with logic for creating new Beats2 directory
			dataPath = null;
			throw new BeatsException(TAG, "Unable to find data path");
		}

		public static bool FileExists(string path) {
			return File.Exists(path);
		}

		public static bool FolderExists(string path) {
			return Directory.Exists(path);
		}

		public static string GetPath(string folder, string filename) {
			return Path.Combine(folder, filename);
		}

		public static string GetParentFolder(string path) {
			DirectoryInfo dir = new DirectoryInfo(path);
			if (dir.Parent != null) {
				return dir.Parent.FullName;
			}
			Logger.Error(TAG, "Unable to determine parent folder of path: " + path);
			return null;
		}

		public static string GetDataPath(string path) {
			return GetPath(dataPath, path);
		}

		public static string GetWwwPath(string path) {
			// I have no idea why the alt separator has to be used here, but otherwise, things break. I blame Unity
			return String.Format("{0}{1}",
				FILE_PROTOCOL,
				Path.Combine(dataPath, path).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
				);
		}

		public static string FindFile(string path, string[] extensions) {
			int index = path.LastIndexOf('.');
			if (index > 0) {
				string checkPath;
				foreach (string extension in extensions) {
					checkPath = path.Substring(0, index) + extension;
					Logger.Debug(TAG, "Searching for file: " + checkPath);
					if (File.Exists(checkPath)) {
						return checkPath;
					}
				}
			}
			if (File.Exists(path)) {
				return path;
			} else {
				return null;
			}
		}
	}
}
