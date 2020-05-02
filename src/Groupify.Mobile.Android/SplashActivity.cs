using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Groupify.Mobile.Droid;

[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true, Icon = "@drawable/AppIcon")]
public class SplashActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var mainIntent = new Intent(this, typeof(MainActivity));
        StartActivity(mainIntent);
        Finish();
        // Create your application here
    }
}