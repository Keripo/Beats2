using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.Common;

/*
 * Last updated: 2012/11/29
 * ~Keripo
 *
 * TODO:
 * - list of work items
 *
 * NOTE:
 * - notes about this file
 * - these coding conventions are a mix of standard C# conventions
 *   plus some personal ones I use for code organization/readability
 */
namespace Beats2.NewNamespace {

	/// <summary>
	/// Description of class
	/// </summary>
	public static class ClassName {
		// TAG const at top of every class
		private const string TAG = "ClassName";
		
		// Enum at top
		public enum EnumName {
			FIRST_ENUM_ITEM,
			SECOND_ENUM_ITEM,
			THIRD_ENUM_ITEM
		}
		
		// Structs after enums
		public struct PublicStruct {
			// 
			public string publicVar { get; private set; }
			public PublicStruct(string parameter) {
				publicVar = parameter;
			}
			public override string OverridingMethod() {
				return String.Format("PublicVar {0}", publicVar);
			}
		}
		
		// Public statics/consts, tab-aligned
		public static string CONSTANT_STRING		= "blah";
		public static string MORE_CONSTANT_STRINGS	= "blah";
		public const int CONST_ID					= 123;
		
		// Private variables, _camelCase except for SETTINGS
		// which we get a local copy for ease of reference
		private static string _privateVar;
		private static int SETTING_NAME;
		
		// One time, app-launch-only initialization stuff
		public static void Init() {
			// Insert initializations
			// Call Reset()
			Reset();
			// Log initialization
			Logger.Debug(TAG, "Initialized...");
		}

		// State reset stuff
		public static void Reset() {
			// Set default values
			_privateVar = "default value";
			// Load settings using SettingsManager
			SETTING_NAME = SettingsManager.GetValueInt(Settings.SETTING_NAME);
			// Log reset
			Logger.Debug(TAG, "Reset...");
		}
		
		// Public function
		public static string PrivateFunction(string paramName) {
			// Declare local variables at top without initialization
			int localVar, moreLocalVar;
			int unrelatedLocalVar;
			string anotherLocalVar;
			List<ClassName> classNameList;
			
			// Body of function, starting with any initializations if needed
			classNameList = new List<ClassName>();
			localVar = Foo(parameterName, _privateVar);
			if (localVar < SETTING_NAME) {
				// Functions from Beats2.* namespace don't need namespace prefix
				// as long as the using line is added up top
				unrelatedLocalVar = Bar(localVar, CONST_ID);
				// Use String.Format instead of lazy "+" unless for very long printed messages
				anotherLocalVar = String.Format("{0}, {1}", localVar, unrelatedLocalVar);
			} else if (localVar > SETTING_NAME) {
				// For any Unity/3rd party classes, be explicit with the namespace
				// The exception is GameObject for convenience
				return UnityEngine.Package.Function(paramName);
			} else {
				// Use StringsManager to fetch strings
				return StringsManager.GetString(Strings.SOME_STRING_VALUE);
			}
			
			// Newline before a return
			return anotherLocalVar;
		}
		
		// Private function
		// If classes are from another Beats2.* package, add a using to the top
		private static EnumName(AnotherEnum n) {
			switch(n) {
				// Case is indented
				case AnotherEnum.LOWEST_ENUM:
				case AnotherEnum.MIDDLE_ENUM:
				// New line for return unless VERY short return call
					return EnumName.FIRST_ENUM_ITEM;
				case AnotherEnum.LAST_ENUM:
					return EnumName.SECOND_ENUM_ITEM;
				// Always separate the default value at bottom
				default:
					return EnumName.THIRD_ENUM_ITEM;
			}
		}
		
	}
	
	// Example base object class
	public abstract class Base {

		// Note that public variables are camelCase instead of PascalCase
		// This is my personal preference, not standard C# coding convention
		public abstract GameObject gameObject { get; set; }
		public abstract float x { get; set; }
		public abstract Vector3 scale { get; set; }
	}
	
	// Example object class
	public class Sprite : Base {

		// Currently just using UnityEngine's GameObject for convenience
		// This Sprite class wraps a GameObject prefab that has a tk2dSprite attached
		// May replace if I figure out how to create prefabs/templates in pure code
		public string publicParameter;
		private GameObject _gameObject;
		private tk2dSprite _sprite;

		// Constructor
		public Sprite(GameObject prefab) {
			// GameObject instantiation in contructor
			_gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
			_sprite = _gameObject.GetComponent<tk2dSprite>();
		}

		// Overrides
		public override GameObject gameObject {
			get { return _gameObject; }
			set { _gameObject = value; }
		}		
		public override Vector3 coords {
			get { return _gameObject.transform.position; }
			set { _gameObject.transform.position = value; }
		}
		public override Vector3 scale {
			get { return _sprite.scale; }
			set { _sprite.scale = value; }
		}
		
		// New public functions declared after constructor and overrides
		public void SpriteSpecificFunction() {
			// Do stuff
			SpriteHelperFunction();
		}
		
		// Helper functions declared after parent function (roughly code-location-wise)
		private void SpriteHelperFunction() {
		}
	}
}
