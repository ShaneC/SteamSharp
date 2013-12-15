using System;

namespace SteamSharp.FlowTests {

	public static class WriteConsole {

		public static void Pause() {
			Console.WriteLine( "Press any key to continue..." );
			Console.ReadKey();
		}

		public static string Prompt( string message ) {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write( "ACTION:\t\t" + message + "\n> " );
			Console.ForegroundColor = ConsoleColor.White;
			return Console.ReadLine();
		}

		public static void Success( string message ) {
			Write( "SUCCESS:\t" + message, ConsoleColor.Green );
		}

		public static void Information( string message ) {
			Write( "INFO:\t\t" + message, ConsoleColor.White );
		}

		public static void Warning( string message ) {
			Write( "WARNING:\t" + message, ConsoleColor.Yellow );
		}

		public static void Error( string message ) {
			Write( "ERROR:\t\t" + message, ConsoleColor.Red );
		}

		public static void Write( string message, ConsoleColor color ) {
			Console.ForegroundColor = color;
			Console.WriteLine( message );
			Console.ForegroundColor = ConsoleColor.White;
		}

	}

}
