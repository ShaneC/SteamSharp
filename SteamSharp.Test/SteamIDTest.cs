﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.Test {

	[TestClass]
	public class SteamIDTest {

		[TestMethod]
		public void SteamIDEquals() {

			SteamID one = new SteamID( "76561198129947779" );
			SteamID two = new SteamID( "76561198129947779" );

			Assert.IsTrue( ( one == two ) );

		}

	}

}
