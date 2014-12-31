/*
 * Copyright (C) 2014, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

using System;

namespace Beats2
{

	public class BeatsException : Exception
	{
		public BeatsException(string message)
            : base(message)
		{
		}

		public BeatsException(string message, Exception innerException)
            : base(message, innerException)
		{
		}
	}
}

