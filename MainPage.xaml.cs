using CommunityToolkit.Maui.Views;
using Plugin.AudioRecorder;
using System;
using System.IO.Compression;
using NAudio;
using NAudio.Wave;

namespace PM2E2GRUPO4
{
    public partial class MainPage : ContentPage
    {

        AudioRecorderService recorder = new AudioRecorderService();
        AudioPlayer player;
        string filePath;
        byte[] audi;
        byte[] i;

        bool va= false;

        public MainPage()
        {
            InitializeComponent();
            GetLocation();
            recorder.TotalAudioTimeout = TimeSpan.FromSeconds(3600);
            recorder.StopRecordingOnSilence = false;
            player = new AudioPlayer();
        }

        async void GetLocation()
        {
            try
            {
                // Solicitar permiso de acceso a la ubicación
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    // Si se obtiene la ubicación, puedes usarla como desees
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    Latitud.Text = latitude.ToString();
                    Longitud.Text = longitude.ToString();
                   

                }
                else
                {
                    await DisplayAlert("Alerta", "GPS no Acivado", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // La geolocalización no es compatible en este dispositivo/platforma
                Console.WriteLine($"Geolocalización no es soportada: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // No se otorgó permiso para acceder a la ubicación
                Console.WriteLine($"Permiso de ubicación no otorgado: {pEx.Message}");
            }
            catch (Exception ex)
            {
                // Otras excepciones
                Console.WriteLine($"Error al obtener la ubicación: {ex.Message}");
            }
        }
        private async void OnStartRecordingButtonClicked(object sender, EventArgs e)
        {
            // Verificar si los permisos de micrófono están concedidos
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                // Si no se conceden los permisos, solicitarlos al usuario
                status = await Permissions.RequestAsync<Permissions.Microphone>();
                if (status != PermissionStatus.Granted)
                {
                    // Si el usuario no concede los permisos, mostrar un mensaje y salir
                    await DisplayAlert("Permiso Requerido", "Se requieren permisos de micrófono para grabar audio.", "Aceptar");
                    return;
                }
            }
            else
            {
                await recorder.StartRecording();
                startRecordingButton.IsEnabled = false;
                stopRecordingButton.IsEnabled = true;
            }
        }

        private async void OnStopRecordingButtonClicked(object sender, EventArgs e)
        {
            await recorder.StopRecording();
            filePath = recorder.GetAudioFilePath();
            audi= ConvertAudioToBase64(filePath);
            Console.WriteLine(audi);
            startRecordingButton.IsEnabled = true;
            stopRecordingButton.IsEnabled = false;
        }

        private async void play(object sender, EventArgs e)
        {
            if (filePath == null)
            {
                await DisplayAlert("Erro", "No hay audio", "ok");
            }
            else
            {
                player.Play(filePath);
            }
        }

        private byte[] ConvertAudioToBase64(string filePath)
        {
            byte[] audio = System.IO.File.ReadAllBytes(filePath);
            return audio;
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

        


        private async void guardar(object sender, EventArgs e)
        {
            validaciones();
            if (va)
            {
                try
                {
                    var image = await drawingView.GetImageStream(200, 200);
                    i = ObtenerImagenDibujada(image);
                    Console.WriteLine(i);
                    var lugar = new Models.lugares
                    {
                        latitud = double.Parse(Latitud.Text),
                        longitud = double.Parse(Longitud.Text),
                        descripcion = Descripcion.Text,
                        firma = i,
                        audio = audi
                    };

                    Models.Msg msg = await Controllers.controladorsitio.CreateEmple(lugar);

                    if (msg != null)
                    {
                        await DisplayAlert("Aviso", msg.message.ToString(), "OK");
                    }
                }catch(Exception ex)
                {
                    await DisplayAlert("Alerta", "Debe llenar todos los campos", "OK");
                }
            }
        }
        private async void lista(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Lista());
        }

        public async void validaciones()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                string latitudText = Latitud.Text;
                string longitudText = Longitud.Text;
                string des = Descripcion.Text;
                if (!string.IsNullOrWhiteSpace(latitudText) && !string.IsNullOrWhiteSpace(longitudText))
                {
                    if(filePath!=null&& !string.IsNullOrWhiteSpace(des))
                    {
                        if (latitudText.Length <= 18 && longitudText.Length <= 18)
                        {
                            va = true;
                        }
                        else
                        {
                            
                            await DisplayAlert("Alerta", "La longitud y latitud deben tener 18 caracteres o menos", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Debe llenar todos los campos", "OK");
                        va = false;
                    }
                }
                else
                {
                    // Al menos uno de los campos no está habilitado
                    await DisplayAlert("Alerta", "por favor llene los campos de ubicacion", "OK");
                    va = false;
                }


                
            }
            else if (current == NetworkAccess.ConstrainedInternet || current == NetworkAccess.Local)
            {
                await DisplayAlert("Aviso", "Conectese a internet", "OK");
                va =  false;
            }
            else
            {
                await DisplayAlert("Aviso", "Conectese a internet", "OK");   
                va= false;
            }
        }
    }
}
