using Android.App; 
using Android.Content; 
using Android.Content.PM;

namespace QCUniversidad.AppClient.Platforms.Android
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionView }, 
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
        DataScheme = CALLBACK_SCHEME)]
    public class QCUniversidadClientAuthenticatorCallbackActivity : WebAuthenticatorCallbackActivity
    {
        const string CALLBACK_SCHEME = "qcuniversidadappclient";
    }
}
