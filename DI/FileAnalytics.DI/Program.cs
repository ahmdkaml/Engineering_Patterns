using System.Data;

public interface IFileReader
{
    DataTable Read();
}

public class CsvFileReader : IFileReader
{
    private readonly string filePath;

    public CsvFileReader(string filePath)
    {
        this.filePath = filePath;
    }

    public DataTable Read()
    {
        var table = new DataTable();

        var lines = File.ReadAllLines(filePath);

        var headers = lines[0].Split(',');

        foreach (var header in headers)
        {
            table.Columns.Add(header);
        }

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var values = line.Split(',');
            table.Rows.Add(values);
        }

        return table;
    }
}

public class Processing
{
    private readonly IFileReader fileReader;

    public Processing(IFileReader fileReader)
    {
        this.fileReader = fileReader;
    }

    public void Run()
    {
        var table = fileReader.Read();

        foreach (DataRow row in table.Rows)
        {
            Console.WriteLine(
                $"{row["sale_id"]} | " +
                $"{row["customer"]} | " +
                $"{row["product"]} | " +
                $"{row["quantity"]} | " +
                $"{row["unit_price"]}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        IFileReader csvReader =
            new CsvFileReader("data/sales.csv");

        var processing = new Processing(csvReader);

        processing.Run();
    }
}
