using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;

namespace PM2E2GRUPO4;
public partial class NewPage1 : ContentPage
{
    Pin pin;
    double lati;
    double loni;
    public NewPage1(double lat, double lon,string des)
    {
		InitializeComponent();
        var map = new Microsoft.Maui.Controls.Maps.Map(MapSpan.FromCenterAndRadius(new Location(lat, lon), Distance.FromMiles(1)));

        // Crear el pin cona ubicación actual
        pin = new Pin
        {
            Label = ""+des,
            Type = PinType.Place,
            Location = new Location(lat, lon)
        };

        // Agregar el pin al mapa
        map.Pins.Add(pin);


        Circle circle = new Circle
        {
            Center = new Location(lat,lon),
            Radius = new Distance(100),
            StrokeColor = Color.FromArgb("#88FF0000"),
            StrokeWidth = 8,
            FillColor = Color.FromArgb("#88FFC0CB")
        };

        // Add the Circle to the map's MapElements collection
        map.MapElements.Add(circle);

        // Establecer el contenido de la página como el mapa
        Content = map;
    }
}