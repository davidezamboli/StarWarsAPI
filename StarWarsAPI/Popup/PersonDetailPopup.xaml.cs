using StarWarsAPI.Models;

namespace StarWarsAPI.Popup;

public partial class PersonDetailPopup : CommunityToolkit.Maui.Views.Popup
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Birth_year { get; set; }
    public string Height { get; set; }
    public string Mass { get; set; }
    public string Population { get; set; }

    public PersonDetailPopup(PeopleResult people)
	{
        Size = new Size(300, 500);
        Name = people.name;
        Gender = people.gender;
        Birth_year = people.birth_year; 
        Height = people.height;
        Mass = people.mass;

        BindingContext = this;
		InitializeComponent();
	}
}