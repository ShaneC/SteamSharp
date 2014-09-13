using System;

namespace SteamSharp {

	/// <summary>
	/// Object representing a Steam ID.
	/// </summary>
	public class SteamID : IComparable<SteamID>, IEquatable<SteamID> {

		public long LongSteamID {
			get;
			private set;
		}

		/// <summary>
		/// Initialize SteamID from string.
		/// </summary>
		/// <param name="steamID"></param>
		public SteamID( string steamID ) {
			_steamID = steamID;
			LongSteamID = Int64.Parse( steamID );
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


		public int CompareTo( SteamID obj ) {
			return this.LongSteamID.CompareTo( obj.LongSteamID );
		}

		/// <summary>
		/// Operator comparator for comparing <see cref="SteamID"/>s.
		/// </summary>
		/// <param name="f1">Input A</param>
		/// <param name="f2">Input B</param>
		/// <returns>True if both SteamIDs match. False otherwise.</returns>
		public static bool operator ==( SteamID f1, SteamID f2 ) {

			if( object.ReferenceEquals( f1, f2 ) )
				return true;
			if( object.ReferenceEquals( f1, null ) ||
				object.ReferenceEquals( f2, null ) ) {
				return false;
			}

			return ( f1.GetHashCode() == f2.GetHashCode() );

		}

		/// <summary>
		/// Operator comparator for comparing <see cref="SteamID"/>s.
		/// </summary>
		/// <param name="f1">Input A</param>
		/// <param name="f2">Input B</param>
		/// <returns>False if both SteamIDs match. True otherwise.</returns>
		public static bool operator !=( SteamID f1, SteamID f2 ) {
			return !( f1 == f2 );
		}

		/// <summary>
		/// Method for determining if two <see cref="SteamID"/>s are equal.
		/// </summary>
		/// <param name="obj">Object to compare against this SteamID.</param>
		/// <returns>False if both SteamIDs match. True otherwise.</returns>
		public override bool Equals( object obj ) {
			return this == ( obj as SteamID );
		}

		/// <summary>
		/// Method for determining if two <see cref="SteamID"/>s are equal.
		/// </summary>
		/// <param name="obj">SteamID to compare against this SteamID.</param>
		/// <returns>False if both SteamIDs match. True otherwise.</returns>
		public bool Equals( SteamID obj ) {
			return this == obj;
		}

	}

}
