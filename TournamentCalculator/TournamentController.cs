using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentCalculator
{
	public class TournamentController
	{
		public TournamentController(int amountPlayers, int amountDevices, double matchDuration)
		{
			this.AmountPlayers = amountPlayers;
			this.AmountDevices = amountDevices;
			this.MatchDuration = matchDuration;
		}

		public int AmountPlayers { get; set; }
		public int AmountDevices { get; set; }
		public double MatchDuration { get; set; }

		public double Simulate()
		{
			double duration = 0.0;
			var matches = DrawMatches();
			while(!IsFinished(matches))
			{
				var playedMatches = new List<Match>();
				for (int i = 0; i < this.AmountDevices; i++)
				{
					bool found = false;
					for (var j = 0; j < matches.Count && !found; j++)
					{
						if (!matches[j].IsFinished && matches[j].IsPlayable && !playedMatches.Contains(matches[j]))
						{
							playedMatches.Add(matches[j]);
							found = true;
						}
					}
				}
				foreach(var match in playedMatches)
				{
					EndMatch(match, matches);
				}
				duration += this.MatchDuration;
			}
			return duration;
		}

		private bool IsFinished(List<Match> matches)
		{
			foreach(var match in matches)
			{
				if (!match.IsFinished)
					return false;
			}
			return true;
		}

		public List<Match> DrawMatches()
		{
			var ret = new List<Match>();
			var orderedPlayers = new List<string>();
			for (var i = 1; i <= this.AmountPlayers; i++)
			{
				orderedPlayers.Add("Spieler " + i);
			}
			var tSize = GetTournamentSize(this.AmountPlayers);
			while (orderedPlayers.Count < tSize)
				orderedPlayers.Add("FL");

			var order = GetOrderForMatches(orderedPlayers.Count);

			for (int i = 0; i < orderedPlayers.Count; i = i + 2)
			{
				var match = new Match(null, orderedPlayers[order[i]], orderedPlayers[order[i + 1]], true, i/2); //new Match(i / 2, orderedPlayers[order[i]], orderedPlayers[order[i + 1]]);
				ret.Add(match);
			}

			var numberOfMatches = (orderedPlayers.Count - 1) * 2;
			for (int i = ret.Count; i < numberOfMatches; i++)
			{
				var match = new Match(i);
				ret.Add(match);
			}
			return ret;
		}


		private List<int> GetOrderForMatches(int playerCount)
		{
			var order = new List<int>() { 0, 1 };
			var actCnt = 2;
			while (actCnt < playerCount)
			{
				actCnt = actCnt * 2;
				for (int i = 0; i < actCnt / 4; i++)
				{
					int pos = 4 * i + 1;
					order.Insert(pos, actCnt - order[pos - 1] - 1);
					order.Insert(pos + 1, actCnt - order[pos + 1] - 1);
				}

			}
			return order;
		}

		public int GetTournamentSize(int players)
		{
			int ret = 2;
			if (players == 0)
				return 0;
			while (ret < players)
				ret = ret * 2;
			return ret;
		}

		public List<int> EndMatch(Match match, List<Match> matches)
		{
			var ret = new List<int>();
			var numberOfMatches = (this.AmountPlayers-1)*2;
			//var numberOfPlayers = tournament.Matches.Count / 2 + 1;
			int winPositionKey = -1;
			int losePositionKey = -1;
			bool isWinnerPlayer1 = true; //is winner of this match Player1 of his next match
			bool isLoserPlayer1 = true; //is loser of this match Player1 of his next match

			match.IsFinished = true;

			if (match.PositionKey < this.AmountPlayers / 2) //first round
			{
				winPositionKey = (match.PositionKey + this.AmountPlayers) / 2; //S1 -> S2
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				losePositionKey = match.PositionKey / 2 + this.AmountPlayers * 3 / 4; //S1 -> V1
				isLoserPlayer1 = match.PositionKey % 2 == 0;

			}
			else if (IsWinnerSide(match.PositionKey, this.AmountPlayers)) // winner side
			{
				var start = this.AmountPlayers / 2;
				var areaSize = this.AmountPlayers / 4;
				var isDesc = true;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize))
				{
					start = start + 3 * areaSize;
					areaSize = areaSize / 2;
					isDesc = !isDesc;
				}

				winPositionKey = (match.PositionKey - start) / 2 + start + 3 * areaSize; //Sx -> Sx+1
				isWinnerPlayer1 = match.PositionKey % 2 == 0;

				if (isDesc)
					losePositionKey = start + 2 * areaSize + (start + areaSize - match.PositionKey - 1);
				else
					losePositionKey = match.PositionKey + 2 * areaSize;
				isLoserPlayer1 = false;
			}
			else if (IsLoserAgainstLoser(match.PositionKey, this.AmountPlayers)) //V(2x+1) -> V(2x+2)
			{
				var start = this.AmountPlayers * 3 / 4;
				var areaSize = this.AmountPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize))
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
					start = start + areaSize;
				}

				winPositionKey = match.PositionKey + areaSize;
				isWinnerPlayer1 = true;
			}
			else //V(2x) -> V(2x+1)
			{
				var start = this.AmountPlayers;
				var areaSize = this.AmountPlayers / 4;
				while (!(match.PositionKey >= start && match.PositionKey < start + areaSize) && areaSize > 0)
				{
					start = start + 2 * areaSize;
					areaSize = areaSize / 2;
				}
				if (areaSize == 0)
					return ret;
				winPositionKey = start + areaSize + areaSize / 2 + (match.PositionKey - start) / 2;
				isWinnerPlayer1 = match.PositionKey % 2 == 0;
				if (match.PositionKey == numberOfMatches - 2)
					isWinnerPlayer1 = false;
			}

			if (isWinnerPlayer1) //set winner
				matches[winPositionKey].Name1 = match.Name1;
			else
				matches[winPositionKey].Name2 = match.Name1;
			ret.Add(winPositionKey);

			if (losePositionKey >= 0) //set loser
			{
				if (isLoserPlayer1)
					matches[losePositionKey].Name1 = match.Name2;
				else
					matches[losePositionKey].Name2 = match.Name2;
				ret.Add(losePositionKey);
			}

			if (!string.IsNullOrEmpty(matches[winPositionKey].Name1) && !string.IsNullOrEmpty(matches[winPositionKey].Name2))
				matches[winPositionKey].IsPlayable = true;
			if (losePositionKey >= 0 && !string.IsNullOrEmpty(matches[losePositionKey].Name1) && !string.IsNullOrEmpty(matches[losePositionKey].Name2))
				matches[losePositionKey].IsPlayable = true;
			return ret;
		}

		private static bool IsLoserAgainstLoser(int positionKey, int numberOfPlayers)
		{
			var start = numberOfPlayers * 3 / 4;
			var area = numberOfPlayers / 4;
			do
			{
				if (positionKey >= start && positionKey < start + area)
					return true;
				start = start + 2 * area;
				area = area / 2;
				start = start + area;
			} while (area > 0);
			return false;
		}

		private static bool IsWinnerSide(int positionKey, int numberOfPlayers)
		{
			var start = numberOfPlayers / 2;
			var areaSize = numberOfPlayers / 4;
			do
			{
				if (positionKey >= start && positionKey < start + areaSize)
					return true;
				start = start + 3 * areaSize;
				areaSize = areaSize / 2;
			} while (areaSize > 0);
			return false;
		}		
	}
}
