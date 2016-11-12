using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TournamentCalculator
{
	public class Match
	{
		#region members
		#endregion

		#region ctors
		public Match(int key)
		{
			this.Dependencies = null;
			this.Name1 = "";
			this.Name2 = "";
			this.IsPlayable = false;
			this.IsFinished = false;
			this.Device = 0;
			this.PositionKey = key;
		}

		public Match(List<Match> dependencies, string name1, string name2, bool isPlayable, int key)
		{
			this.Dependencies = dependencies;
			this.Name1 = name1;
			this.Name2 = name2;
			this.IsPlayable = isPlayable;
			this.IsFinished = false;
			this.Device = 0;
			this.PositionKey = key;
		}
		#endregion

		#region properties
		public List<Match> Dependencies { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public bool IsPlayable { get; set; }
		public bool IsFinished { get; set; }
		public int Device { get; set; }
		public int PositionKey { get; set; }
		#endregion

		#region private methods
		#endregion

		#region public methods
		#endregion
 }
}