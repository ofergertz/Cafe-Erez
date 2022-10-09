using Microsoft.JSInterop;

namespace CafeErez.Client.Shared
{
    public partial class NavMenu
    {
        const string SportResults = "https://www.flashscore.com";

        const string LottoResult = "https://www.flashscore.com";

        const string Pais123Result = "https://www.flashscore.com";

        const string Pais777Result = "https://www.flashscore.com";

        const string ChanceResult = "https://www.flashscore.com";

        const string Winner = "http://www.winner.co.il";

        private async Task DirectTo(string url)
        {
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");
        }
    }
}
