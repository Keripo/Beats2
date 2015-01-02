/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */

namespace Beats2
{

	public class Pair<T, U>
	{
		public T key;
		public U value;
		public Pair(T key, U value)
		{
			this.key = key;
			this.value = value;
		}
	}
}

