using Plugin.AudioRecorder;
using System.Collections.ObjectModel;

namespace PM2E2GRUPO4;

public partial class Lista : ContentPage
{
    int id;
    string des;
    double lat;
    double lon;
    byte[] firm;
    byte[] data;

    AudioPlayer player;
    string tempAudioFile;
    public ObservableCollection<Models.Lugaresget> Posts { get; set; }
    public Lista()
	{
		InitializeComponent();
        Posts = new ObservableCollection<Models.Lugaresget>();
        LoadData();
        BindingContext = this;
        player = new AudioPlayer();
    }
    private async void PostListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
            return;


        var selectedPost = e.Item as Models.Lugaresget;

        id = selectedPost.id;
        lat = selectedPost.latitud;
        lon = selectedPost.longitud;
        data = selectedPost.audio;
        firm= selectedPost.firma;
        des = selectedPost.descripcion;
    }
        public async void LoadData()
    {
        var posts = await Controllers.controladorsitio.GetPosts();
        if (posts != null)
        {
            foreach (var post in posts)
            {
                byte[] firmaBytes = post.firma;

                
                string firmaBase64 = Convert.ToBase64String(firmaBytes);

                Posts.Add(post);
            }
        }
        else
        {

        }
    }

    private async void Button1_Clicked(object sender, EventArgs e)
    {
        if (id != 0)
        {
            await Navigation.PushAsync(new up(id, lat, lon, firm, des));
            Navigation.RemovePage(this);
        }
        else
        {
            await DisplayAlert("Aviso", "seleccione un elemento de la lista", "ok");
        }
    }

    private async void Button2_Clicked(object sender, EventArgs e)
    {
        if (data != null)
        {
            tempAudioFile = Path.Combine(FileSystem.CacheDirectory, "temp_audio.mp3");
            File.WriteAllBytes(tempAudioFile, data);
            player.Play(tempAudioFile);
        }
        else
        {
            await DisplayAlert("Aviso", "seleccione un elemento de la lista", "ok");
        }
    }

    private async void Button3_Clicked(object sender, EventArgs e)
    {
        if (lat != 0)
        {
            await Navigation.PushAsync(new NewPage1(lat, lon, des));
        }
        else
        {
            await DisplayAlert("Aviso", "Seleccione un elemento de la lista","ok");
        }
        
    }
}