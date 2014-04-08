
namespace Beats2.Data
{
    public class PatternInfo
    {
        public PatternType type;
        public PatternMode mode;
        public int keyCount;

        public PatternDifficulty difficulty;
        public string difficultyName;
        public int difficultyValue;
        public PatternRadar radar;

        public string credits;
        public string description;
    }

    public enum PatternType
    {
        Beats,
        Technika,
        Square,
        Taiko,
        Mai,
        Diva,
        Osu
    }

    public enum PatternMode
    {
        Pad,
        Touch,
        Keyboard
    }

    // See https://github.com/stepmania/stepmania/blob/master/src/Difficulty.cpp
    public enum PatternDifficulty
    {
        Tutorial,
        Beginner,
        Easy, // Basic, Light
        Medium, // Another, Trick, Standard, Difficult
        Hard, // SSR, Maniac, Heavy
        Challenge, // SManiac, Expert, Oni
        Edit,
        Unknown
    }

    public struct PatternRadar
    {
        public int stream;
        public int voltage;
        public int air;
        public int freeze;
        public int chaos;
    }

}
