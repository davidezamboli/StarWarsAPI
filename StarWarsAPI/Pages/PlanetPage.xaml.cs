using CommunityToolkit.Maui.Views;
using StarWarsAPI.Models;
using StarWarsAPI.Popup;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace StarWarsAPI.Pages;

public partial class PlanetPage : ContentPage
{
    string _planetBaseUrl = "https://swapi.dev/api/planets/?page=2";
    private readonly HttpClient _httpClient;

    private PlanetResponse _planetResponse;
    public ObservableCollection<PlanetResult> Planets { get; set; } = new();

    public bool IsLoading { get; set; }
    public bool IsSearching { get; set; }

    public ICommand ShowPlanet => new Command<PlanetResult>((planet) => ShowPlanetDetails(planet));
    public ICommand SearchCommand => new Command<string>(async (searchTerm) => await ExecuteSearchCommand(searchTerm));


    public PlanetPage()
	{
		InitializeComponent();
        BindingContext = this;
        _httpClient = new HttpClient();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        IsSearching = false;
        OnPropertyChanged(nameof(IsSearching));

        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        await LoadPlanets();

        IsLoading = false;
        OnPropertyChanged(nameof(IsLoading));
    }

    private async Task LoadPlanets()
    {
        _planetResponse = await _httpClient.GetFromJsonAsync<PlanetResponse>(_planetBaseUrl);

        Planets.Clear();

        foreach (var planet in _planetResponse.results)
        {
            Planets.Add(planet);
        }
    }

    private void ShowPlanetDetails(PlanetResult planet)
    {
        var planetPopup = new PlanetDetailPopup(planet);
        this.ShowPopup(planetPopup);
    }

    private async Task ExecuteSearchCommand(string searchTerm)
    {
        IsSearching = true;
        OnPropertyChanged(nameof(IsSearching));
        //await Task.Delay(1000);

        if (string.IsNullOrEmpty(searchTerm))
        {
            await LoadPlanets();
        }
        else
        {
            Planets = await SearchPlanet(searchTerm);
        }

        IsSearching = false;
        OnPropertyChanged(nameof(IsSearching));

        OnPropertyChanged(nameof(Planets));
    }

    public Task<ObservableCollection<PlanetResult>> SearchPlanet(string searchTerm)
    {
        IEnumerable<PlanetResult> filteredPlanet = string.IsNullOrEmpty(searchTerm)
           ? Planets
           : Planets.Where(planets => planets.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        ObservableCollection<PlanetResult> filteredPlanetCollection = new ObservableCollection<PlanetResult>(filteredPlanet);
        return Task.FromResult(filteredPlanetCollection);
    }

    private async void PlanetSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        IsSearching = true;
        OnPropertyChanged(nameof(IsSearching));
        //await Task.Delay(1000);

        if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            SearchCommand.Execute(null);
        }

        IsSearching = false;
        OnPropertyChanged(nameof(IsSearching));
    }
}