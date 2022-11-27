using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using System.Windows.Input;
using HtmlDataAnalyzer.Core;
using AsyncAwaitBestPractices.MVVM;
using HtmlDataAnalyzer.Core.Model;
using System.Windows;

namespace HtmlDataAnalyzer.App.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly BrowserDataExtractor browserDataExtractor;

        private bool isLoading;
        private string? url;
        private int waitingTimeMs;
        private ICollection<KeyValuePair<string, int>>? words;
        private ICollection<ImageDataModel>? images;

        public bool IsLoading
        {
            get => isLoading;
            set => SetField(ref isLoading, value);
        }

        public string? Url
        {
            get => url;
            set => SetField(ref url, value);
        }

        public int WaitingTimeMs
        {
            get => waitingTimeMs;
            set => SetField(ref waitingTimeMs, value);
        }

        public ICollection<KeyValuePair<string, int>>? Words
        {
            get => words;
            set => SetField(ref words, value);
        }

        public ICollection<ImageDataModel>? Images
        {
            get => images;
            set => SetField(ref images, value);
        }

        public ICommand InitCommand { get; }
        public AsyncCommand RunAnalysis { get; set; }

        public MainViewModel()
        {
            browserDataExtractor = new BrowserDataExtractor();

            Url = "https://instagram.com";

            InitCommand = new AsyncCommand(ExecuteInitAsync);
            RunAnalysis = new AsyncCommand(RunAnalysisAsyncSafe);
        }

        private async Task ExecuteInitAsync()
        {
            IsLoading = true;
            await browserDataExtractor.InitializeBrowser();
            IsLoading = false;
        }

        private async Task RunAnalysisAsyncSafe()
        {
            IsLoading = true;
            try
            {
                await RunAnalysisAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error just occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            IsLoading = false;
        }

        private async Task RunAnalysisAsync()
        {
            Debug.Assert(Url != null, nameof(Url) + " != null");

            var result = await browserDataExtractor.ExecuteAnalyzingAsync(
                Url, 
                TimeSpan.FromMilliseconds(WaitingTimeMs));

            Images = result.Images;
            Words = result.Words
                ?.OrderByDescending(x => x.Value)
                .Take(10)
                .ToList();
        }
    }
}
