﻿using System;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using Youtube_DL.Core;
using Youtube_DL.Model;
using Youtube_DL.View;

namespace Youtube_DL.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        
        private AddVideoPopup viewPopup;
        #region Property

        public bool Isloading { get; set; }
        public bool ShowHsVideoText => MainVideoList.Count > 0;

        public ISnackbarMessageQueue Notifications { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
        public ObservableCollection<YoutubeVideoModel> MainVideoList { get; set; }

        #endregion Property

        #region Command

        public Command _AddButton { get; }
        public Command _AddToClipboard { get; }


        #endregion Command

        public MainViewModel()
        {
            MainVideoList = new ObservableCollection<YoutubeVideoModel>();
            viewPopup = new AddVideoPopup();
            _AddButton = new Command(AddViewShow);
            _AddToClipboard = new Command(AddToClipboard);
            MainVideoList.CollectionChanged += (o,s) => OnPropertyChanged(nameof(ShowHsVideoText));
        }

        private async void AddToClipboard()
        {
            if (Clipboard.ContainsText())
            {
                string ClipBoardText = Clipboard.GetText();

                if (AddVideoPopupViewModel.CheckURL(ClipBoardText))
                {
                    try
                    {
                        //var Info = await DialogHost.Show(new LoadingView(), "MainDialog");
                        //MainVideoList.Add((YoutubeVideoModel)(Info ?? Info ));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Ссылка не валидна");
                }
            }
        }

        private async void AddViewShow()
        {
            try
            {
                var videoInfo = await DialogHost.Show(viewPopup);

                if (videoInfo is null || videoInfo is not YoutubeVideoModel) return;

                MainVideoList.Add(videoInfo as YoutubeVideoModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}