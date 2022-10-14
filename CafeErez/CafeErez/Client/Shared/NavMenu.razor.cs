using Microsoft.JSInterop;

namespace CafeErez.Client.Shared
{
    public partial class NavMenu
    {
        const string SportResults = "https://www.flashscore.com";

        const string LottoResult = "https://www.pais.co.il/lotto/archive.aspx";

        const string Pais123Result = "https://www.pais.co.il/123/archive.aspx";

        const string Pais777Result = "https://www.pais.co.il/777/archive.aspx";

        const string ChanceResult = "https://www.pais.co.il/chance/archive.aspx";

        const string Winner = "http://www.winner.co.il";

        private async Task DirectTo(string url)
        {
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");
        }
    }
}
