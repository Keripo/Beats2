/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

namespace Beats2.Data
{

	public struct Metadata
	{
		public string simfileParentDirectoryPath;
		public string simfilePath;
		public string simfileVersion;
		public string simfileDatabaseId;
        
		public string infoCredits;
		public string infoPack;
		public string infoDescription;
		public string infoCategory;
		public string infoTags;
        
		public string songTitle;
		public string songTitleTranslit;
		public string songSubtitle;
		public string songSubtitleTranslit;
		public string songArtist;
		public string songArtistTranslit;
		public string songAlbum;
		public string songAlbumTranslit;
		public string songSeries;
		public string songSeriesTranslit;
		public string songGenre;
		public string songReleaseYear;
        
		public string musicPath;
		public string musicOffset;
		public string musicSampleStart;
		public string musicSampleLength;
		public string musicBpmDisplay;
        
		public string graphicBackground;
		public string graphicBanner;
		public string graphicCover;
	}
}