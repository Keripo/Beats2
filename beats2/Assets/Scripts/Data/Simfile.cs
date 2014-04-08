/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System.Collections.Generic;

namespace Beats2.Data
{
    // Simfile data structure layout
    // Simfile
    // - Metadata
    // - Lyrics
    // - Charts
    //   - ChartInfo
    //   - Events
    //   - Notes
    public class Simfile
    {
        public Metadata metadata;
        public List<Lyric> lyrics;
        public List<Chart> charts;
    }
}
