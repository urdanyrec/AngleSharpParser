using AngleSharp;
using AngleSharp.Dom;
using AngleSharpParser;
using CsvHelper;
using System.Globalization;
Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Task> task = new List<Task>();

//количество страниц
int pages = 11; 

string cellSelector = "d-block p-1 product-name gtm-click";
List<string> ItemCellSelector = new List<string>() { "a[data-src='#region']", "nav.breadcrumb", "h1.detail-name",
                                                     "span.price", "span.old-price", "span.ok", "div.card-slider-nav img.img-fluid" };
string baseAddress = "https://www.toy.ru";
string catalogAdress = "/catalog/boy_transport/?filterseccode%5B0%5D=transport&PAGEN_5="; //+page number

IConfiguration config = Configuration.Default.WithDefaultLoader();
IBrowsingContext context = BrowsingContext.New(config);

for (int i = 0; i < pages; i++)
{
    int s = i;
    task.Add(new Task(() => ParsePage(s+1)));
}

foreach(var t in task)
{
    t.Start();
    Thread.Sleep(500);
}

async Task ParsePage(int pg)
{ 
    List<Items> names = new List<Items>();
    List<ItemCard> itemCards = new List<ItemCard>();

    int page = pg;
    IDocument document = await context.OpenAsync(baseAddress + catalogAdress + page);

    IHtmlCollection<IElement> cells = document.GetElementsByClassName(cellSelector);//document.QuerySelectorAll(cellSelector);
    IEnumerable<string> titles = cells.Select(m => m.GetAttribute("href"));

    foreach (string title in titles)
    {
        names.Add(new Items(baseAddress + title, page.ToString(), cellSelector));//page = title;                                                                                                          //Console.WriteLine(title);
    }

    for (int j = 0; j < names.Count; j++)
    {
        string url = names[j].Url;
        var document2 = await context.OpenAsync(url);

        List<string> buf = new List<string>();
        List<string> bufImg = new List<string>();

        for (int i = 0; i < ItemCellSelector.Count; i++)
        {
            var cells0 = document2.QuerySelectorAll(ItemCellSelector[i]);
            IEnumerable<string> titles0 = cells0.Select(m => m.TextContent);
            string rawString;
            if (i == 6)
            {
                titles0 = cells0.Select(m => m.GetAttribute("src"));
                foreach (string title in titles0) bufImg.Add(title);
            }
            try
            {
                rawString = titles0.ElementAt(0);
            }
            catch
            {
                rawString = "";
            }
            if (i == 0)
            {
                rawString = new string(titles0.ElementAt(0).Where(c => !char.IsControl(c)).ToArray());//rawString.Replace(char., string.Empty);
                rawString = rawString.Replace(" ", string.Empty);
            }

            buf.Add(rawString);

        }
        itemCards.Add(new ItemCard(buf[0], buf[1], buf[2], buf[3], buf[4], buf[5], string.Join(" ", bufImg), names[j].Url)); //[0]
        names[j].itemcard = new ItemCard(buf[0], buf[1], buf[2], buf[3], buf[4], buf[5], string.Join(" ", bufImg), names[j].Url);//itemCards;

        Console.WriteLine(names[j].itemcard.Name);
    }

    Console.WriteLine($"Страница: {page} запаршена");

    var csvPath = @"C:\ParseData.csv";
    using (var streamWriter = new StreamWriter(csvPath, true))
    {
        using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(itemCards);
        }
    }
}

await Task.WhenAll(task);

bool b = true;
while (b == true)
{
    if (!task[0].IsCompleted) b = false;
    else { Thread.Sleep(1000); b = true; }
}