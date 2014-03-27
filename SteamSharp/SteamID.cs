
namespace SteamSharp {

	/// <summary>
	/// Object representing a Steam ID.
	/// </summary>
	public class SteamID {

		/// <summary>
		/// Initialize SteamID from string.
		/// </summary>
		/// <param name="steamID"></param>
		public SteamID( string steamID ) {
			_steamID = steamID;
		}
		private string _steamID;

		public override string ToString() {
			return _steamID;
		}

		/// <summary>
		/// Generates a list of <see cref="SteamID"/> objects from an input array of strings.
		/// </summary>
		/// <param name="steamIDs">String representation of a 64-bit SteamID.</param>
		/// <returns>List of <see cref="SteamID"/> objects representing the converted values of the input array.</returns>
		public static SteamID[] CreateFromList( string[] steamIDs ) {
			SteamID[] arr = new SteamID[steamIDs.Length];
			for( int i = 0; i < steamIDs.Length; i++ )
				arr[i] = new SteamID( steamIDs[i] );
			return arr;
		}

		public override int GetHashCode() {
			return ToString().GetHashCode();
		}

		public override bool Equals( object obj ) {
			return ToString().Equals( obj.ToString() );
		}

	}

}
