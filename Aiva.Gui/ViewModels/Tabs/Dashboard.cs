using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aiva.Core.Twitch;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Linq;
using PropertyChanged;
using System.Windows.Input;
using TwitchLib.Client.Extensions;

namespace Aiva.Gui.ViewModels.Tabs {
    [AddINotifyPropertyChangedInterface]
    public class Dashboard {
        public int TotalViewers { get; set; }
        public SeriesCollection CurrentViewersSeries { get; set; }
        public bool IsTotalViewersGraphActive { get; set; } = true;

        public string StreamTitle { get; set; }
        public string SelectedGame { get; set; }
        public List<string> StreamGames { get; set; }

        public int TotalViews { get; set; }
        public int TotalFollowers { get; set; }

        public List<string> CommercialTypes { get; set; }

        public TwitchLib.Client.Enums.CommercialLength SelectedCommercial { get; set; }

        public ICommand ChangeGameCommand { get; set; }
        public ICommand ChangeStreamTitleCommand { get; set; }
        public ICommand ShowCommercialCommand { get; set; }

        private bool subsOnly;
        public bool SubsOnly {
            get { return subsOnly; }
            set { SetSubsOnly(value); subsOnly = value; }
        }

        private bool slowMode;
        public bool SlowMode {
            get { return slowMode; }
            set { SetSlowMode(value); slowMode = value; }
        }
        
        private bool _isPartnered;

        public Dashboard() {
            AivaClient.TwitchClient.OnExistingUsersDetected
                += (s, e)
                => TotalViewers += e.Users.Count;

            AivaClient.TwitchClient.OnUserJoined
                += (s, e)
                => TotalViewers++;

            AivaClient.TwitchClient.OnUserLeft
                += (s, e)
                => TotalViewers--;

            AivaClient.TwitchClient.OnChannelStateChanged
                += (s, e)
                => {
                    if (e.ChannelState.SlowMode.HasValue) {
                        SlowMode = e.ChannelState.SlowMode.Value != 0;
                    }
                    if (e.ChannelState.SubOnly.HasValue) {
                        SubsOnly = e.ChannelState.SubOnly.Value;
                    }

                };

            CurrentViewersSeries = new SeriesCollection {
                new LineSeries {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue> {
                        new ObservableValue(TotalViewers)
                    }
                }
            };

            ChangeGameCommand = new Internal.AsyncRelayCommand(
                new Func<object, Task>(async (game)
                => await AivaClient.TwitchApi.V5.Channels.UpdateChannelAsync(
                    channelId: Core.ConfigHandler.Config.General.ChannelID,
                    game: SelectedGame,
                    authToken: Core.ConfigHandler.Config.Credentials.StreamerOAuthToken).ConfigureAwait(false)));

            ChangeStreamTitleCommand = new Internal.AsyncRelayCommand(
                new Func<object, Task>(async (game)
                => await AivaClient.TwitchApi.V5.Channels.UpdateChannelAsync(
                    channelId: Core.ConfigHandler.Config.General.ChannelID,
                    status: StreamTitle,
                    authToken: Core.ConfigHandler.Config.Credentials.StreamerOAuthToken).ConfigureAwait(false)));

            ShowCommercialCommand = new Internal.RelayCommand(
                com => AivaClient.TwitchClient.StartCommercial(
                    Core.ConfigHandler.Config.General.Channel,
                    SelectedCommercial),
                com => _isPartnered);

            CommercialTypes = new List<string>();
            foreach(var e in Enum.GetValues(typeof(TwitchLib.Client.Enums.CommercialLength))) {
                CommercialTypes.Add(e.ToString());
            }

            UpdateCurrentViewers();
            GetStreamGames();
            UpdateChannelDetailsFrequently();
            GetChannelStatesFrequently();
        }

        private void GetChannelStatesFrequently() {

        }

        private void SetSubsOnly(bool value) {
            if(value) {
                AivaClient.TwitchClient.SubscribersOnlyOn(
                    Core.ConfigHandler.Config.General.Channel);
            } else {
                AivaClient.TwitchClient.SubscribersOnlyOff(
                    Core.ConfigHandler.Config.General.Channel);
            }
        }

        private void SetSlowMode(bool value) {
            if (value) {
                AivaClient.TwitchClient.SlowModeOn(
                    Core.ConfigHandler.Config.General.Channel,
                    TimeSpan.FromSeconds(3));
            } else {
                AivaClient.TwitchClient.SlowModeOff(
                    Core.ConfigHandler.Config.General.Channel);
            }
        }

        private async void UpdateChannelDetailsFrequently() {
            while(true) {
                var channel = await AivaClient.TwitchApi.V5.Channels.GetChannelByIDAsync(
                    Core.ConfigHandler.Config.General.ChannelID)
                    .ConfigureAwait(false);

                if (channel != null) {
                    StreamTitle = channel.Status;
                    SelectedGame = channel.Game;
                    TotalViews = channel.Views;
                    TotalFollowers = channel.Followers;
                    _isPartnered = channel.Partner;
                }

                await Task.Delay(TimeSpan.FromMinutes(5))
                    .ConfigureAwait(false);
            }
        }

        private async void GetStreamGames() {
            var games = await AivaClient.TwitchApi.V5.Games.GetTopGamesAsync(100)
                .ConfigureAwait(false);

            if(games != null) {
                StreamGames = new List<string>(games.Top.OrderBy(o => o.Game.Popularity).Select(s => s.Game.Name));
            }
        }

        /// <summary>
        /// Updates the TotalViewers chart frequently
        /// </summary>
        private async void UpdateCurrentViewers() {
            while (true) {
                await Task.Delay(5000)
                    .ConfigureAwait(false);

                CurrentViewersSeries[0].Values.Add(new ObservableValue(TotalViewers));

                if (CurrentViewersSeries[0].Values.Count >= 20) {
                    CurrentViewersSeries[0].Values.RemoveAt(0);
                }
            }
        }
    }
}
