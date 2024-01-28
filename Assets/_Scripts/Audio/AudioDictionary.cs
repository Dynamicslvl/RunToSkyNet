using System.Collections.Generic;

public static class AudioDictionary
{
	public static List<string> names = new List<string> {
		"Bonus",
		"BulletHit",
		"Clap",
		"Crash",
		"Home",
		"Laugh",
		"Lose",
		"Music",
		"Police",
		"Pop cat",
		"Spring",
		"Star",
		"Tap",
		"Null",
	};

	public static List<float> lengths = new List<float> {
		0.816f,
		0.72f,
		2.016f,
		2.616f,
		5.232f,
		0.912f,
		9.456f,
		217.104f,
		2.16f,
		1.56f,
		0.456f,
		0.6395238f,
		0.04845805f,
		0f,
	};
}

public enum AudioEnum
{
	Bonus,
	BulletHit,
	Clap,
	Crash,
	Home,
	Laugh,
	Lose,
	Music,
	Police,
	PopCat,
	Spring,
	Star,
	Tap,
	Null,
}