using StarWarsAPI.Models;
namespace StarWarsAPI.Popup;

public partial class PlanetDetailPopup : CommunityToolkit.Maui.Views.Popup
{
    public List<string> TerrainList { get; set; } = new();
    public string Name { get; set; }
    public string Rotation_period { get; set; }
    public string Orbital_period { get; set; }
    public string Climate { get; set; }
    public string Gravity { get; set; }
    public string Terrain { get; set; }
    public string Population { get; set; }

    public PlanetDetailPopup(PlanetResult planet)
    {
        Size = new Size(300, 500);
        Name = planet.name;
        Rotation_period = planet.rotation_period;
        Orbital_period = planet.orbital_period;
        Climate = planet.climate;
        Gravity = planet.gravity;
        TerrainList = planet.terrain.Split(',').ToList();
        Population = planet.population;

        BindingContext = this;
        InitializeComponent();
    }
}