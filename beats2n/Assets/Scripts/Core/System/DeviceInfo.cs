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

using UnityEngine;

namespace Beats2.Core {
	
	/// <summary>
	/// Device info. Wraps Unity's SystemInfo class.
	/// </summary>
	public static class DeviceInfo {
		
		private const string TAG = "DeviceInfo";
		
		public static float MAX_PHONE_SCREEN_WIDTH = 3;
		
		public enum DeviceTypes {
			METRO,
			DESKTOP,
			CONSOLE,
			WEB,
			TABLET,
			PHONE
		}

		public enum Platforms {
			METRO_ARM,
			METRO_X64,
			METRO_X86,
			DESKTOP_WINDOWS,
			DESKTOP_OSX,
			DESKTOP_LINUX,
			CONSOLE_WII,
			CONSOLE_XBOX,
			CONSOLE_PS3,
			WEB_BROWSER,
			WEB_GOOGLE,
			WEB_FLASH,
			DEVICE_ANDROID,
			DEVICE_IOS,
			DEVICE_WINPHONE
		}

		public static string appName			{ get; private set; }
		public static string appVersionNum		{ get; private set; }
		public static string appVersionName		{ get; private set; }

		public static string deviceName			{ get; private set; }
		public static string deviceModel		{ get; private set; }
		public static string deviceId			{ get; private set; }

		public static Platforms platform		{ get; private set; }
		public static DeviceTypes deviceType	{ get; private set; }
		public static string operatingSystem	{ get; private set; }
		public static string unityVersion		{ get; private set; }

		public static bool vibrationSupport		{ get; private set; }
		public static bool touchSupport			{ get; private set; }

		public static SystemLanguage language	{ get; private set; }
		
		public static float width				{ get; private set; }
		public static float height				{ get; private set; }
		public static float min					{ get; private set; }
		public static float dpi					{ get; private set; }
		public static float widthPhysical		{ get; private set; }
		public static float heightPhysical		{ get; private set; }
		public static float minPhysical			{ get; private set; }
		
		/// <summary>
		/// Return a printable string describing the device
		/// </summary>
		public static string GetInfo() {
			return
				"appName: " + appName +
				"\nappVersionNum: " + appVersionNum +
				"\nappVersionName: " + appVersionName +
				"\ndeviceName: " + deviceName +
				"\ndeviceModel: " + deviceModel +
				"\ndeviceId: " + deviceId +
				"\nplatform: " + platform +
				"\ndeviceType: " + deviceType +
				"\noperatingSystem: " + operatingSystem +
				"\nunityVersion: " + unityVersion +
				"\nvibrationSupport: " + vibrationSupport +
				"\ntouchSupport: " + touchSupport +
				"\nsystemLanguage: " + language +
				"\ndataPath: " + Loader.GetDataPath();
			;
		}
		
		/// <summary>
		/// Load device info from system
		/// </summary>
		public static void LoadInfo() {
			LoadScreenInfo();
			LoadPlatformAndDeviceType();
			
			appName = Constants.APP_NAME;
			appVersionNum = Constants.APP_VERSION_NUM;
			appVersionName = Constants.APP_VERSION_NAME;

			deviceName = SystemInfo.deviceName;
			deviceModel = SystemInfo.deviceModel;
			deviceId = SystemInfo.deviceUniqueIdentifier;
			
			operatingSystem = SystemInfo.operatingSystem;
			unityVersion = Application.unityVersion;

			vibrationSupport = SystemInfo.supportsVibration;
			touchSupport = Input.multiTouchEnabled;	
			language = Application.systemLanguage;
		}
		
		/// <summary>
		/// Load screen info
		/// </summary>
		public static void LoadScreenInfo() {
			width = (float)UnityEngine.Screen.width;
			height = (float)UnityEngine.Screen.height;
			min = (width < height) ? width : height;
			dpi = (UnityEngine.Screen.dpi > 0) ? UnityEngine.Screen.dpi : 1f;
			widthPhysical = width / dpi;
			heightPhysical = height / dpi;
			minPhysical = (widthPhysical < heightPhysical) ? widthPhysical : heightPhysical;
		}
		
		/// <summary>
		/// Returns whether or not the device is a phone based on physical screen size
		/// </summary>
		private static bool IsPhone() {
			return minPhysical < MAX_PHONE_SCREEN_WIDTH;
		}
		
		/// <summary>
		/// Load the platforma and device types
		/// </summary>
		private static void LoadPlatformAndDeviceType() {
			switch (Application.platform) {
				case RuntimePlatform.MetroPlayerARM:
					platform = Platforms.METRO_ARM;
					deviceType = DeviceTypes.METRO;
					break;
				case RuntimePlatform.MetroPlayerX64:
					platform = Platforms.METRO_X64;
					deviceType = DeviceTypes.METRO;
					break;
				case RuntimePlatform.MetroPlayerX86:
					platform = Platforms.METRO_X86;
					deviceType = DeviceTypes.METRO;
					break;
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.OSXDashboardPlayer:
					platform = Platforms.DESKTOP_OSX;
					deviceType = DeviceTypes.DESKTOP;
					break;
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.WindowsPlayer:
					platform = Platforms.DESKTOP_WINDOWS;
					deviceType = DeviceTypes.DESKTOP;
					break;
				case RuntimePlatform.LinuxPlayer:
					platform = Platforms.DESKTOP_LINUX;
					deviceType = DeviceTypes.DESKTOP;
					break;
				case RuntimePlatform.OSXWebPlayer:
				case RuntimePlatform.WindowsWebPlayer:
					platform = Platforms.WEB_BROWSER;
					deviceType = DeviceTypes.WEB;
					break;
				case RuntimePlatform.FlashPlayer:
					platform = Platforms.WEB_FLASH;
					deviceType = DeviceTypes.WEB;
					break;
				case RuntimePlatform.NaCl:
					platform = Platforms.WEB_GOOGLE;
					deviceType = DeviceTypes.WEB;
					break;
				/*
				case RuntimePlatform.WiiPlayer:
					platform = Platforms.CONSOLE_WII;
					deviceType = DeviceTypes.CONSOLE;
					break;
				*/
				case RuntimePlatform.XBOX360:
					platform = Platforms.CONSOLE_XBOX;
					deviceType = DeviceTypes.CONSOLE;
					break;
				case RuntimePlatform.PS3:
					platform = Platforms.CONSOLE_PS3;
					deviceType = DeviceTypes.CONSOLE;
					break;
				case RuntimePlatform.IPhonePlayer:
					platform = Platforms.DEVICE_IOS;
					deviceType = IsPhone() ? DeviceTypes.PHONE : DeviceTypes.TABLET;
					break;
				case RuntimePlatform.Android:
					platform = Platforms.DEVICE_ANDROID;
					deviceType = IsPhone() ? DeviceTypes.PHONE : DeviceTypes.TABLET;
					break;
				case RuntimePlatform.WP8Player:
					platform = Platforms.DEVICE_WINPHONE;
					deviceType = DeviceTypes.PHONE;
					break;
				default:
					platform = Platforms.DESKTOP_WINDOWS;
					deviceType = DeviceTypes.DESKTOP;
					break;
			}
		}
	}
}
