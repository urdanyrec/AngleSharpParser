namespace AngleSharpParser
{
    public class ItemCard
    {
        public string Region { get; set; } //;

        public string BreadCrumbs { get; set; } //;

        public string Name { get; set; }//;

        public string Price { get; set; }//;

        public string OldPrice { get; set; }// = "";

        public string Availability { get; set; }//;

        //public List<string> picturesUrl { get; set; }

        public string PicturesUrl { get; set; }//;//List<string> picturesUrl;

        public string Url { get; set; }//;//List<string> picturesUrl;

        public ItemCard(string region, string kroshki, string name, string price, string oldPrice, string availability, string picturesUrl, string url)//List<string> picturesUrl)////
        {
            this.Region = region;
            this.BreadCrumbs = kroshki;
            this.Name = name;
            this.Price = price;
            this.OldPrice = oldPrice;
            this.Availability = availability;
            this.PicturesUrl = picturesUrl;
            this.Url = url;
        }

        public string getInfo()
        {
            return ("region: " + Region + " , kroshki: " + BreadCrumbs + " , name: " + Name + " , price: " + Price
                     + " , old price: " + OldPrice + " , availability: " + Availability + " , picturesUrl: " + PicturesUrl);//picturesUrl.ToArray() ); //", name: " + tag +
        }
    }
}
