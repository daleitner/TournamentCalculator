using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Windows.Input;
using Base;

namespace TournamentCalculator
{
	public class MainViewModel : ViewModelBase
	{
        private const int ThrowDuration = 25; //2 Spieler, jeweils 3 Darts, insgesamt 25 sekunden
		private const double OffsetDuration = 3; //3 Minuten Wartezeit pro Durchgang (Aufruf, bullwurf usw)
		#region members
		private RelayCommand calculateCommand;
		private List<int> playerAverages;
		private List<string> legOptions;
		private List<string> setOptions;
		private List<string> matchOptions;
		private List<string> modeOptions;
		private int amountPlayers;
		private int selectedPlayerAverage;
		private int amountDevices;
        private int hour = 19;
        private int minutes;
		private string selectedLegOption = "";
		private string selectedSetOption = "";
		private string selectedMatchOption = "";
		private string selectedModeOption = "";
		private string legDuration = "";
		private string setDuration = "";
		private string matchDuration = "";
		private string fullDuration = "";
		private string endTime = "";
		#endregion

		#region ctors
		public MainViewModel()
		{
            InitializeData();
		}

        private void InitializeData()
        {
            this.legOptions = new List<string> {"301", "501"};
            this.selectedLegOption = this.legOptions[1];

            this.setOptions = new List<string> { "1 Leg" };
            for (var i = 3; i < 12; i += 2)
                this.setOptions.Add("Best of " + i + " Legs");
            this.selectedSetOption = this.setOptions[1];

            this.matchOptions = new List<string> { "1 Set" };
            for (var i = 3; i < 8; i += 2)
                this.matchOptions.Add("Best of " + i + " Sets");
            this.selectedMatchOption = this.matchOptions[0];

            this.modeOptions = new List<string> { "KO Modus", "Doppel KO", "Round Robin", "Round Robin + KO", "Round Robin + DKO", "Ziehung mit Nachkauf" };
            this.selectedModeOption = this.modeOptions[1];

            this.playerAverages = new List<int> { 35, 40, 45, 50, 60, 70, 80, 100};
            this.selectedPlayerAverage = 45;

	        this.amountDevices = 1;
	        this.amountPlayers = 16;
        }
        #endregion

        #region properties
        public ICommand CalculateCommand
		{
			get
			{
				if (this.calculateCommand == null)
				{
					this.calculateCommand = new RelayCommand(
						param => Calculate(),
                        param => CanCalculate()
					);
				}
				return this.calculateCommand;
			}
		}

        public List<int> PlayerAverages
		{
			get
			{
				return this.playerAverages;
			}
			set
			{
				this.playerAverages = value;
				OnPropertyChanged("PlayerAverages");
			}
		}
		public List<string> LegOptions
		{
			get
			{
				return this.legOptions;
			}
			set
			{
				this.legOptions = value;
				OnPropertyChanged("LegOptions");
			}
		}
		public List<string> SetOptions
		{
			get
			{
				return this.setOptions;
			}
			set
			{
				this.setOptions = value;
				OnPropertyChanged("SetOptions");
			}
		}
		public List<string> MatchOptions
		{
			get
			{
				return this.matchOptions;
			}
			set
			{
				this.matchOptions = value;
				OnPropertyChanged("MatchOptions");
			}
		}
		public List<string> ModeOptions
		{
			get
			{
				return this.modeOptions;
			}
			set
			{
				this.modeOptions = value;
				OnPropertyChanged("ModeOptions");
			}
		}
		public string AmountPlayers
		{
			get
			{
				return this.amountPlayers.ToString();
			}
			set
			{
                if (!Int32.TryParse(value, out this.amountPlayers))
                    this.amountPlayers = 0;
				OnPropertyChanged("AmountPlayers");
			}
		}
		public int SelectedPlayerAverage
		{
			get
			{
				return this.selectedPlayerAverage;
			}
			set
			{
				this.selectedPlayerAverage = value;
				OnPropertyChanged("SelectedPlayerAverage");
			}
		}
		public string AmountDevices
		{
			get
			{
				return this.amountDevices.ToString();
			}
			set
			{
                if (!Int32.TryParse(value, out this.amountDevices))
                    this.amountDevices = 0;
				OnPropertyChanged("AmountDevices");
			}
		}
		public string Hour
		{
			get
			{
				return this.hour.ToString();
			}
			set
			{
                if (!Int32.TryParse(value, out this.hour))
                    this.hour = 0;
				OnPropertyChanged("Hour");
			}
		}
		public string Minutes
		{
			get
			{
				return this.minutes.ToString();
			}
			set
			{
                if (!Int32.TryParse(value, out this.minutes))
                    this.minutes = 0;
				OnPropertyChanged("Minutes");
			}
		}
		public string SelectedLegOption
		{
			get
			{
				return this.selectedLegOption;
			}
			set
			{
				this.selectedLegOption = value;
				OnPropertyChanged("SelectedLegOption");
			}
		}
		public string SelectedSetOption
		{
			get
			{
				return this.selectedSetOption;
			}
			set
			{
				this.selectedSetOption = value;
				OnPropertyChanged("SelectedSetOption");
			}
		}
		public string SelectedMatchOption
		{
			get
			{
				return this.selectedMatchOption;
			}
			set
			{
				this.selectedMatchOption = value;
				OnPropertyChanged("SelectedMatchOption");
			}
		}
		public string SelectedModeOption
		{
			get
			{
				return this.selectedModeOption;
			}
			set
			{
				this.selectedModeOption = value;
				OnPropertyChanged("SelectedModeOption");
			}
		}
		public string LegDuration
		{
			get
			{
				return this.legDuration;
			}
			set
			{
				this.legDuration = value;
				OnPropertyChanged("LegDuration");
			}
		}
		public string SetDuration
		{
			get
			{
				return this.setDuration;
			}
			set
			{
				this.setDuration = value;
				OnPropertyChanged("SetDuration");
			}
		}
		public string MatchDuration
		{
			get
			{
				return this.matchDuration;
			}
			set
			{
				this.matchDuration = value;
				OnPropertyChanged("MatchDuration");
			}
		}
		public string FullDuration
		{
			get
			{
				return this.fullDuration;
			}
			set
			{
				this.fullDuration = value;
				OnPropertyChanged("FullDuration");
			}
		}
		public string EndTime
		{
			get
			{
				return this.endTime;
			}
			set
			{
				this.endTime = value;
				OnPropertyChanged("EndTime");
			}
		}
		#endregion

		#region private methods
		private void Calculate()
		{
            int legPoints;
            Int32.TryParse(this.SelectedLegOption, out legPoints);

            var roundsPerLeg = legPoints / this.SelectedPlayerAverage;
            var nLegDuration = (double)roundsPerLeg * ThrowDuration / 60;
			nLegDuration = Math.Round(nLegDuration, 1);
            this.LegDuration = "Leg Dauer: " + nLegDuration + " min";

			var avgLegs = (double)(3*GetNumberOfBestOfString(this.SelectedSetOption)+1)/4; //((x+1)/2 + x)/2
			if (avgLegs < 0)
				avgLegs = 1;
			var nSetDuration = nLegDuration*avgLegs;
			nSetDuration = Math.Round(nSetDuration, 1);
			this.SetDuration = "Set Dauer: " + nSetDuration + " min";

			var avgSets = (double)(3 * GetNumberOfBestOfString(this.SelectedMatchOption) + 1) / 4; //((x+1)/2 + x)/2
			if (avgSets < 0)
				avgSets = 1;
			var nMatchDuration = nSetDuration * avgSets;
			nMatchDuration = Math.Round(nMatchDuration, 1);
			this.MatchDuration = "Match Dauer: " + nMatchDuration + " min";

			switch (this.SelectedModeOption)
			{
				case "Doppel KO":
					CalculateDKO(nMatchDuration);
					break;
			}
		}

		private void CalculateDKO(double nMatchDuration)
		{
			var c = new TournamentController(this.amountPlayers, this.amountDevices, nMatchDuration, OffsetDuration);
			var totalDuration = c.Simulate();
			var totalHour = (int)totalDuration/60;
			var totalMinutes = totalDuration - totalHour*60;
			this.FullDuration = "Gesamtdauer: " + totalHour + " h " + totalMinutes + " min";
			var endDate = new DateTime(2000,1,1,this.hour, this.minutes, 0);
			endDate = endDate.AddMinutes(totalDuration);
			this.EndTime = "Spielende: " + endDate.ToString("HH:mm") + " Uhr";
		}

        private bool CanCalculate()
        {
            if (string.IsNullOrEmpty(this.SelectedLegOption))
                return false;
            if (string.IsNullOrEmpty(this.SelectedSetOption))
                return false;
            if (string.IsNullOrEmpty(this.SelectedMatchOption))
                return false;
            if (string.IsNullOrEmpty(this.SelectedModeOption))
                return false;
            if (this.selectedPlayerAverage == 0)
                return false;

            if (this.amountPlayers < 2)
                return false;
            if (this.amountDevices < 1)
                return false;
            if (this.hour < 0 || this.hour > 23)
                return false;
            if (this.minutes < 0 || this.minutes > 59)
                return false;
            return true;
        }

		private int GetNumberOfBestOfString(string s)
		{
			int ret;
			var arr = s.Split(' ');
			if (arr.Length < 3)
				return -1;
			Int32.TryParse(arr[2], out ret);
			return ret;
		}
        #endregion

        #region public methods
        #endregion
    }
}
