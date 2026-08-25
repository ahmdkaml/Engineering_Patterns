using System.Text.Json;
using System.Xml.Linq;

public class Sale
{
    public string SaleId { get; set; }
    public string Customer { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Sale(string saleId, string customer, string product, int quantity, decimal unitPrice)
    {
        SaleId = saleId;
        Customer = customer;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}

class ProcessFile
{
    private string extension;
    private string filePath;

    public ProcessFile(string filePath)
    {
        this.filePath = filePath;
        this.extension = Path.GetExtension(filePath);
    }

    public void PrintResult()
    {
        var result = readResult();

        foreach (var sale in result)
        {
            Console.WriteLine(
                $"{sale.SaleId} | " +
                $"{sale.Customer} | " +
                $"{sale.Product} | " +
                $"{sale.Quantity} | " +
                $"{sale.UnitPrice}");
        }
    }

    private List<Sale> readResult()
    {
        if (extension == ".csv")
        {
            var lines = File.ReadAllLines(filePath);

            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var values = line.Split(',');

                    return new Sale(
                        values[0],
                        values[1],
                        values[2],
                        int.Parse(values[3]),
                        decimal.Parse(values[4]));
                })
                .ToList();
        }

        if (extension == ".json")
        {
            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Sale>>(json)
                   ?? throw new InvalidOperationException("Invalid JSON.");
        }

        if (extension == ".xml")
        {
            var document = XDocument.Load(filePath);

            return document
                .Root!
                .Elements("sale")
                .Select(sale => new Sale(
                    sale.Element("saleId")!.Value,
                    sale.Element("customer")!.Value,
                    sale.Element("product")!.Value,
                    int.Parse(sale.Element("quantity")!.Value),
                    decimal.Parse(sale.Element("unitPrice")!.Value)))
                .ToList();
        }

        throw new NotSupportedException(
            $"Unsupported file extension: {extension}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var csv = new ProcessFile("data/sales.csv");

        var json = new ProcessFile("data/sales.json");

        var xml = new ProcessFile("data/sales.xml");

        csv.PrintResult();
        json.PrintResult();
        xml.PrintResult();
    }
}
