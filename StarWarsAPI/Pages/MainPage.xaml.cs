using CommunityToolkit.Maui.Views;
using StarWarsAPI.Models;
using StarWarsAPI.Popup;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace StarWarsAPI.Pages;

public partial class MainPage : ContentPage
{
    //bettere with a baseUrl = "https://swapi.dev/api/"
    string _peopleBaseUrl = "https://swapi.dev/api/people/?page=2";
	//string _planetBaseUrl = "https://swapi.dev/api/planets/?page=2";
    private readonly HttpClient _httpClient;

    private PeopleResponse _peopleResponse;
	//private PlanetResponse _planetResponse;
    public ObservableCollection<PeopleResult> People { get; set; } = new();
    //public ObservableCollection<PlanetResult> Planets { get; set; } = new();
    public bool IsLoading { get; set; }
    public bool IsSearching { get; set; }
    //public bool IsPeopleListVisible { get; set; } = true;
    //public bool IsPlanetsListVisible { get; set; } = false;

    //public ICommand ShowPlanet => new Command<PlanetResult>((planet) => ShowPlanetDetails(planet));
    public ICommand ShowPeople => new Command<PeopleResult>((people) => ShowPeopleDetails(people));
    public ICommand SearchCommand => new Command<string>(async (searchTerm) => await ExecuteSearchCommand(searchTerm));
    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
        _httpClient = new HttpClient();
        //ButtonSetVisibilityList.Text = "Planets";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //OnPropertyChanged(nameof(IsPeopleListVisible));

        IsSearching = false;
        OnPropertyChanged(nameof(IsSearching));

        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        //await Task.WhenAll(LoadPeople(), LoadPlanets());
        await LoadPeople();

        IsLoading = false;
        OnPropertyChanged(nameof(IsLoading));
    }

    private async Task LoadPeople()
    {
        _peopleResponse = await _httpClient.GetFromJsonAsync<PeopleResponse>(_peopleBaseUrl);

        People.Clear();

        foreach (var person in _peopleResponse.results)
        {
            People.Add(person);
        }
    }

    //private async Task LoadPlanets()
    //{
    //    _planetResponse = await _httpClient.GetFromJsonAsync<PlanetResponse>(_planetBaseUrl);

    //    Planets.Clear();

    //    foreach (var planet in _planetResponse.results)
    //    {
    //        Planets.Add(planet);
    //    }
    //}

    //private void ShowPlanetDetails(PlanetResult planet)
    //{
    //    var planetPopup = new PlanetDetailPopup(planet);
    //    this.ShowPopup(planetPopup);
    //}

    private void ShowPeopleDetails(PeopleResult people)
    {
        var peoplePopup = new PersonDetailPopup(people);
        this.ShowPopup(peoplePopup);
    }

    //private void Button_Clicked(object sender, EventArgs e)
    //{
    //    IsPeopleListVisible = !IsPeopleListVisible;
    //    IsPlanetsListVisible = !IsPlanetsListVisible;
    //    OnPropertyChanged(nameof(IsPeopleListVisible));
    //    OnPropertyChanged(nameof(IsPlanetsListVisible));

    //    if (IsPlanetsListVisible)
    //    {
    //        ButtonSetVisibilityList.Text = "People";
    //        Title = "Planets";
    //    }
    //    if (IsPeopleListVisible)
    //    {
    //        ButtonSetVisibilityList.Text = "Planets";
    //        Title = "People";
    //    }
    //}

    private async Task ExecuteSearchCommand(string searchTerm)
    {
        IsSearching = true;
        OnPropertyChanged(nameof(IsSearching));
        //await Task.Delay(1000);

        if (string.IsNullOrEmpty(searchTerm))
        {
            //await Task.WhenAll(LoadPeople(), LoadPlanets());
            await LoadPeople();
        }
        else
        {
            //People = await SearchPeople(searchTerm);
            await SearchPersons(searchTerm);
            //Planets = await SearchPlanet(searchTerm);
        }

        IsSearching = false;
        OnPropertyChanged(nameof(IsSearching));

        OnPropertyChanged(nameof(People));
        //OnPropertyChanged(nameof(Planets));
    }

    public async Task SearchPersons(string searchTerm)
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var filteredPeople = People.Where(people => people.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            People.Clear();
            foreach (var person in filteredPeople)
            {
                People.Add(person);
            }
            //var filteredPeople = People.Where(people => people.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        }
        else
        {
            await LoadPeople();

        }
    }

    public Task <ObservableCollection<PeopleResult>> SearchPeople(string searchTerm)
    {
        IEnumerable<PeopleResult> filteredPeople = string.IsNullOrEmpty(searchTerm)
           ? People
           : People.Where(people => people.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        ObservableCollection<PeopleResult> filteredPeopleCollection = new ObservableCollection<PeopleResult>(filteredPeople);
        return Task.FromResult(filteredPeopleCollection);
    }

    //public Task<ObservableCollection<PlanetResult>> SearchPlanet(string searchTerm)
    //{
    //    IEnumerable<PlanetResult> filteredPlanet = string.IsNullOrEmpty(searchTerm)
    //       ? Planets
    //       : Planets.Where(planets => planets.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

    //    ObservableCollection<PlanetResult> filteredPlanetCollection = new ObservableCollection<PlanetResult>(filteredPlanet);
    //    return Task.FromResult(filteredPlanetCollection);
    //}

    private async void MainSearchBar_TextChanged(object sender, TextChangedEventArgs e)
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