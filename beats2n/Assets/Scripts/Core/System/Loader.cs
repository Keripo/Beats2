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
using UnityEngine;

namespace Beats2.Core {
	
	/// <summary>
	/// File loader and other IO functions
	/// </summary>
	public static class Loader {
		
		private const string TAG = "Loader";
		
		#region File/folder IO functions
		/// <summary>
		/// Check if a file exists
		/// </summary>
		public static bool FileExists(string path) {
#if UNITY_METRO && !UNITY_EDITOR
			// TODO METRO
			return true;
#else
			return File.Exists(path);
#endif
		}
		
		/// <summary>
		/// Check if a folder exists
		/// </summary>
		public static bool FolderExists(string path) {
#if UNITY_METRO && !UNITY_EDITOR
			// TODO METRO
			return true;
#else
			return Directory.Exists(path);
#endif
		}
		
		/// <summary>
		/// Check if a folder is empty or not
		/// </summary>
		public static bool IsEmptyFolder(string path) {
#if UNITY_METRO && !UNITY_EDITOR
			// TODO METRO
			return true;
#else
			return Directory.Exists(path)
				&& Directory.GetDirectories(path).Length == 0
				&& Directory.GetFiles(path).Length == 0;
#endif
		}
		
		/// <summary>
		/// Gets the parent path of a folder
		/// </summary>
		public static string GetParentPath(string path) {
#if UNITY_METRO && !UNITY_EDITOR
			// TODO METRO
			return null;
#else
			if (Directory.Exists(path)) {
				return Directory.GetParent(path).FullName;
			} else {
				Logger.Warning(TAG, "Unable to get parent path for: {0}", path);
				return null;
			}
#endif
		}
		     
		/// <summary>
		/// Finds a file given the path and list of supported file extensions
		/// </summary>
		public static string FindFile(string path, string[] extensions) {
			Logger.Debug(TAG, "Searching for file: {0}", path);
			
			// Check for file extension
			int index = path.LastIndexOf('.');
			if (index > 0) {
				// Check with current file extension
				string currentExtension = path.Substring(index + 1).ToLower();
				foreach (string extension in extensions) {
					if (String.Equals(currentExtension, extension, StringComparison.OrdinalIgnoreCase)) {
						if (FileExists(path)) {
							return path;
						} else {
							break;
						}
					}
				}
				// Check against other supported file extensions
				string checkPath;
				foreach (string extension in extensions) {
					checkPath = path.Substring(0, index) + extension;
					if (FileExists(checkPath)) {
						return checkPath;
					}
				}
			}
			// Last try
			if (FileExists(path)) {
				return path;
			} else {
				return null;
			}
		}
		#endregion
		
		#region Path functions
		/// <summary>
		/// Returns a WWW compatible file path
		/// </summary>
		public static string GetWwwPath(string path) {
			// I have no idea why the alt separator has to be used here, but otherwise, things break. I blame Unity
			return String.Format("{0}{1}",
				FILE_PROTOCOL,
				path.Replace('\\', '/')
				);
		}
		
		/// <summary>
		/// Returns the data path where the Beats2 folder will be created
		/// </summary>
		public static string GetDataPath() {
			if (_dataPath != null) {
				return _dataPath;
			} else {

#if UNITY_ANDROID && !UNITY_EDITOR
				// Android SD card
				foreach (string path in SDCARD_PATHS) {
					if (!IsEmptyFolder(path)) {
						_dataPath = path;
						break;
					}
				}
#else				
				// Editor root folder or Windows exe folder
				_dataPath = GetParentPath(Application.dataPath);
#endif
				
				if (_dataPath != null) {
					return _dataPath;
				} else {
					return "<dataPath>";
					//throw new BeatsException(TAG, "Unable to determine data path");
				}
			}
		}
		
		#endregion
	
		#region File loading functions
		
		/// <summary>
		/// Loads an audio file as an audio clip
		/// </summary>
		public static AudioClip LoadAudioClip(string path, bool stream) {
			Logger.Debug(TAG, "Loading audio file: {0}", path);
			
			// Get file path
			string filePath = FindFile(path, AUDIO_EXTENSIONS);
			if (filePath == null) {
				Logger.Error(TAG, "Unable to find audio file: {0}", path);
				return null;
			}
			
			// Load url
			string url = GetWwwPath(filePath);
			WWW www = new WWW(url);
			
			// Load audio clip
			AudioClip clip = www.GetAudioClip(false, stream); // No 3D audio
			while (!clip.isReadyToPlay); // Wait for buffer
			//www.Dispose(); // FIXME: Is this needed?
		
			// Return loaded audio clip
			if (clip == null) {
				Logger.Error(TAG, "Failed to load audio file: {0}", path);
			}
			return clip;
		}
		
		/// <summary>
		/// Load an image file as a texture
		/// </summary>
		public static Texture2D LoadTexture(string path, bool repeat) {
			Logger.Debug(TAG, "Loading image file: {0}", path);
			
			// Get file path
			string filePath = FindFile(path, IMAGE_EXTENSIONS);
			if (filePath == null) {
				Logger.Error(TAG, "Unable to find image file: {0}", path);
				return null;
			}
			
			// Load url
			string url = GetWwwPath(filePath);
			WWW www = new WWW(url);
			while (!www.isDone); // Wait until file is downloaded
			
			// Load texture
			Texture2D texture = www.texture;
			texture.wrapMode = (repeat) ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
			texture.Compress(true); // High quality compression
		
			// Return loaded texture
			if (texture == null) {
				Logger.Error(TAG, "Failed to load texture file: {0}", path);
			}
			return texture;
		}
		
		/// <summary>
		/// Load a text file as a string
		/// </summary>
		public static string LoadText(string path) {
			Logger.Debug(TAG, "Loading text file: {0}", path);
			
			// Load url
			string url = GetWwwPath(path);
			WWW www = new WWW(url);
			while (!www.isDone); // Wait until file is downloaded
			
			// Return loaded text (could be null)
			return www.text;
		}
		
		/// <summary>
		/// Take a screenshot
		/// </summary>
		public static void TakeScreenshot() {
			string fileName = String.Format("{0}.png", DateTime.Now.ToString());
			UnityEngine.Application.CaptureScreenshot(fileName);
			Logger.Log(TAG, "Screenshot saved: {0}", fileName);
		}
		
		#endregion
		
		#region Private variables/constants
		
		private const string FILE_PROTOCOL = "file://";
		
		/// <summary>
		/// Supported audio file extensions
		/// </summary>
		private static string[] AUDIO_EXTENSIONS = {
			".mp3",
			".ogg",
			".wav"
		};
		
		/// <summary>
		/// Supported image file extensions
		/// </summary>
		private static string[] IMAGE_EXTENSIONS = {
			".png",
			".jpg",
			".bmp"
		};
		
#if UNITY_ANDROID && !UNITY_EDITOR
		/// <summary>
		/// Manual exhaustive list of external SD card locations
		/// </summary>
		private static string[] SDCARD_PATHS = {
			"/sdcard/legacy",
			"/sdcard/0",
			"/sdcard",
			"/mnt/sdcard/external_sd",
			"/mnt/sdcard/external",
			"/mnt/sdcard/ext_sd",
			"/mnt/sdcard",
			"/mnt/sd",
			"/mnt/external_sd",
			"/mnt/external",
			"/mnt/ext_sd"
		};
#endif
		
		// Private variables
		private static string _dataPath;
		
		#endregion
		
	}
}
