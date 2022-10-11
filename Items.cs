namespace AngleSharpParser
{
    public class Items
    {
        public string Url { get; set; } //;

        public string Page { get; set; } //;

        public string CssClass { get; set; } //;

        public ItemCard itemcard { get; set; } //;


        public Items(string url, string page, string tag)
        {
            this.Url = url;
            this.Page = page;
            this.CssClass = tag;
        }

        public string getInfo()
        {
            return ("url: " + Url + " , page: " + Page); //", name: " + tag +
        }
    }
}
