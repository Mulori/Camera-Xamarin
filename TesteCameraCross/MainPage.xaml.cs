using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TesteCameraCross
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await TakePhotoAsync();
        }

        async Task TakePhotoAsync()
        {
            try
            {

                try
                {
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Teste Cmera", ex.ToString(), "Ok");
                }

                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro", "Este dispositivo não tem suporte a essa funcionalidade", "Entendi");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro", "Este dispositivo não tem permissão a essa funcionalidade", "Entendi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            string photoPath;

            // canceled
            if (photo == null)
            {
                photoPath = null;
                return;
                
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            photoPath = newFile;

            imagemView.Source = photoPath;
        }
    }
}
