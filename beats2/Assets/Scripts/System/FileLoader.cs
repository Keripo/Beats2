/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace Beats2
{

	public static class FileLoader
	{
		private const string TAG = "FileLoader";
		private const string BEATS2_DIR = "Beats2";
		private const string FILE_PROTOCOL = "file://";

		public static string[] AUDIO_EXTENSIONS = {
		// TODO: Temporary fix til I use an FMOD plugin for LoadAudio
#if UNITY_ANDROID && !UNITY_EDITOR
			".mp3",
#else
			".ogg",
#endif
			".wav"
		};
		public static string[] IMAGE_EXTENSIONS = {
			".png",
			".jpg",
			".bmp"
		};
		public static string[] LYRICS_EXTENSIONS = {
			".lrc"
		};

#if UNITY_ANDROID && !UNITY_EDITOR
		// TODO: Figure out better way that is compatible with
		// Android API 21 (Android 5.0 Lollipop)
		private static string[] SDCARD_PATHS = {
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

		private static string _dataPath;

		public static string FindFile(string path, string[] extensions, bool restrictExtensions = false)
		{
			Logger.Debug(TAG, "Searching for file: {0}", path);
			if (string.IsNullOrEmpty(path)) {
				Logger.Warn(TAG, "Unable to search with an empty path");
				return null;
			}

			string foundPath = null;
			string currentExtension = GetFileExtension(path);

			// Check original path
			if (!restrictExtensions || currentExtension == null) {
				if (FileExists(path)) {
					foundPath = path;
				}
			} else {
				// Check only if current extension is in extension list
				if (currentExtension != null) {
					foreach (string extension in extensions) {
						if (String.Equals(currentExtension, extension, StringComparison.OrdinalIgnoreCase)) {
							if (FileExists(path)) {
								foundPath = path;
							}
							break;
						}
					}
				}
			}

			// Check with different extensions
			if (foundPath == null && currentExtension != null) {
				string checkPath;
				foreach (string extension in extensions) {
					checkPath = path.Replace(currentExtension, extension);
					if (FileExists(checkPath)) {
						foundPath = checkPath;
						break;
					}
				}
			}

			// Log and return result
			if (foundPath != null) {
				Logger.Debug(TAG, "Found file: {0}", foundPath);
			} else {
				Logger.Error(TAG, "Unable to find file: {0}", path);
			}
			return foundPath;
		}

		public static string GetPath(string filename, string parentFolderPath)
		{
			return FixPath(parentFolderPath) + Path.DirectorySeparatorChar + filename.TrimStart(Path.DirectorySeparatorChar);
		}
		
		public static bool FileExists(string path)
		{
			return File.Exists(FixPath(path));
		}
		
		public static string GetFileExtension(string path)
		{
			return Path.GetExtension(path);
		}

		public static string GetParentFolder(string path)
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			if (dir.Parent != null) {
				return dir.Parent.FullName;
			}
			Logger.Error(TAG, "Unable to determine parent folder of path: {0}", path);
			return null;
		}
		
		public static AudioClip LoadAudioClip(string path, bool stream = false)
		{
			Logger.Debug(TAG, "Loading audio file: {0}", path);
			
			// Get file path
			string filePath = FindFile(path, AUDIO_EXTENSIONS, true);
			if (filePath == null) {
				Logger.Error(TAG, "Unable to find audio file: {0}", path);
				return null;
			}
			
			// Load url
			string url = GetWwwPath(filePath);
			using (WWW www = new WWW(url)) {
				// Load audio clip
				AudioClip clip = www.GetAudioClip(false, stream); // No 3D audio
				while (!clip.isReadyToPlay) {
					// Wait for buffer
				}
				if (clip == null) {
					Logger.Error(TAG, "Failed to load audio file: {0}", path);
				}
				return clip;
			}
		}
		
		public static Texture2D LoadTexture(string path, bool repeat = false)
		{
			Logger.Debug(TAG, "Loading image file: {0}", path);
			
			// Get file path
			string filePath = FindFile(path, IMAGE_EXTENSIONS);
			if (filePath == null) {
				Logger.Error(TAG, "Unable to find image file: {0}", path);
				return null;
			}
			
			// Load url
			string url = GetWwwPath(filePath);
			using (WWW www = new WWW(url)) {
				while (!www.isDone) {
					// Wait until file is downloaded
				}
			
				// Load texture
				Texture2D texture = www.texture;
				texture.wrapMode = (repeat) ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
				texture.Compress(true); // High quality compression
				if (texture == null) {
					Logger.Error(TAG, "Failed to load texture file: {0}", path);
				}
				return texture;
			}
		}
		
		public static string LoadText(string path)
		{
			Logger.Debug(TAG, "Loading text file: {0}", path);
			
			// Load url
			string url = GetWwwPath(path);
			using (WWW www = new WWW(url)) {
				while (!www.isDone) {
					// Wait until file is downloaded
				}

				// Load text
				String text = www.text;
				if (text == null) {
					Logger.Error(TAG, "Failed to load text file: {0}", path);
				}
				return text;
			}
		}

		private static string GetWwwPath(string path)
		{
			// URLs only support '/' it seems
			return String.Format("{0}{1}", FILE_PROTOCOL, path.Replace('\\', '/'));
		}

		public static string GetDataPath(string path)
		{
			return Path.Combine(GetDataPath(), FixPath(path));
		}

		public static string FixPath(string path)
		{
			return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar);
		}
		
		private static string GetDataPath()
		{
			if (_dataPath == null) {
				SetDataPath();
			}
			return _dataPath;
		}
		
		private static void SetDataPath()
		{
			// Find an existing Beats2 directory
#if UNITY_ANDROID && !UNITY_EDITOR
			foreach (string path in SDCARD_PATHS) {
				_dataPath = Path.Combine(path, BEATS2_DIR);
				if (Directory.Exists(_dataPath)) {
					return;
				}
			}
#else
			string path = Application.dataPath;
			DirectoryInfo dir = new DirectoryInfo(path);
			if (dir.Parent != null) {
				path = dir.Parent.FullName;
			}
			_dataPath = Path.Combine(path, BEATS2_DIR);
			if (Directory.Exists(_dataPath)) {
				return;
			}
#endif
			// TODO - replace this with logic for creating new Beats2 directory
			_dataPath = null;
			throw new BeatsException("Unable to find data path");
		}
	}
}