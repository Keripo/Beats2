
/*
 * TODO:
 * - keep updated
 */
namespace Beats2.System {

	/// <summary>
	/// Scene names. Keep this sync'd with level numbers
	/// </summary>
	public enum Scenes {
		HOME,
		SPLASH,
		GAME_SELECT,
		SONG_SELECT,
		RESULTS
	}

	/// <summary>
	/// Tags. Keep this synced manually with the Unity Editor
	/// </summary>
	public static class Tags {
		public static string UNTAGGED				= "Untagged";
		public static string CAMERA					= "MainCamera";
		public static string MENU_LOGO				= "Menu_Logo";
		public static string MENU_MUSIC_PLAYER		= "Menu_MusicPlayer";
		public static string SANDBOX_TEST_ARROW		= "Sandbox_TestArrow";
		public static string SANDBOX_TEST_MINE		= "Sandbox_TestMine";
		public static string SANDBOX_TEST_HOLD		= "Sandbox_TestHold";
	}

	/// <summary>
	/// Constants. Immutable values and non-localize strings
 	/// </summary>
	public static class Constants {
		public static string APP_NAME				= "Beats2";
		public static string APP_VERSION_NUM		= "1";
		public static string APP_VERSION_NAME		= "TEST-VER";		
	}
	
}
