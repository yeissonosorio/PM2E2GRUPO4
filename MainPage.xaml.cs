using Plugin.AudioRecorder;
using System;

namespace PM2E2GRUPO4
{
    public partial class MainPage : ContentPage
    {

        AudioRecorderService recorder = new AudioRecorderService();
        AudioPlayer player;
        string filePath;
        string au;

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
                    Longitud.IsEnabled = false;
                    Latitud.IsEnabled = false;

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
            au = ConvertAudioToBase64(filePath);
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

        private string ConvertAudioToBase64(string filePath)
        {
            byte[] audioBytes = File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(audioBytes);
            return base64String;
        }

        private async void guardar(object sender, EventArgs e)
        {
            
            var lugar = new Models.lugares
            {
                latitud = double.Parse(Latitud.Text),
                longitud = double.Parse(Longitud.Text),
                descripcion = Descripcion.Text,
                firma = au,
                audio = au
            };
            
            Models.Msg msg = await Controllers.controladorsitio.CreateEmple(lugar);

            if (msg != null)
            {
                await DisplayAlert("Aviso", msg.message.ToString(), "OK");
            }
        }
    }
}
