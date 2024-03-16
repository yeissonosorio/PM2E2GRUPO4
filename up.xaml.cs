namespace PM2E2GRUPO4;

public partial class up : ContentPage
{
	byte[] data;
	int I;
	public up(int id,double lat,double lon, byte[] fir,string des)
	{
		InitializeComponent();
		data = fir;
		I= id;
		Latitud.Text = lat+"";
		Longitud.Text = lon + "";
		Descripcion.Text = des;
		Latitud.IsEnabled = false;
		Longitud.IsEnabled=false;
	}

	private async void actu(object sender, EventArgs e)
	{
        try
        {
            var image = await drawingView.GetImageStream(200, 200);
        byte[] i= ObtenerImagenDibujada(image);
		var ac = new Models.update
		{
			firma = i,
			descripcion = Descripcion.Text
		};
		Models.Msg msg = await Controllers.controladorsitio.UpdateSitio(I+"",ac);
        if (msg != null)
        {
            await DisplayAlert("Aviso", msg.message.ToString(), "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alerta", "Debe llenar todos los campos", "OK");
        }
        await Navigation.PushAsync(new Lista());
        Navigation.RemovePage(this);
    }
    private async void elimi(object sender, EventArgs e)
    {
        bool confirmacion = await DisplayAlert("Confirmación", "¿Está seguro de eliminar este registro?", "Sí", "No");
        if (confirmacion)
        {
            Models.Msg msg = await Controllers.controladorsitio.EliminarSitio(I+"");
            if (msg != null)
            {
                await DisplayAlert("Aviso", msg.message.ToString(), "OK");
            }
            await Navigation.PushAsync(new Lista());
            Navigation.RemovePage(this);
        }
        else
        {
            
        }
    }

    private byte[] ObtenerImagenDibujada(Stream imagen)
    {


        byte[] imagenBytes;
        using (MemoryStream ms = new MemoryStream())
        {
            imagen.CopyTo(ms);
            imagenBytes = ms.ToArray();
        }
        return imagenBytes;
    }
}