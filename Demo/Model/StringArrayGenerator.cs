using Bogus;
using System;

public class StringArrayGenerator
{
    public string[][] GenerateStringArray(int rows = 1000, int columns = 8)
    {
        var faker = new Faker();

        string[][] result = new string[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new string[columns];

            // Column 0: Random name
            result[i][0] = faker.Name.FullName();

            // Column 1: Random email
            result[i][1] = faker.Internet.Email();

            // Column 2: Random phone number
            result[i][2] = faker.Phone.PhoneNumber("###-###-####");

            // Column 3: Random address
            result[i][3] = faker.Address.FullAddress();

            // Column 4: Random company
            result[i][4] = faker.Company.CompanyName();

            // Column 5: Random date (as string)
            result[i][5] = faker.Date.Past(10).ToString("yyyy-MM-dd");

            // Column 6: Random product name
            result[i][6] = faker.Commerce.ProductName();

            // Column 7: Random sentence
            result[i][7] = faker.Lorem.Sentence();
        }

        return result;
    }

    // Alternative: More customizable version with different data types
    public string[][] GenerateCustomStringArray(int rows = 1000, int columns = 8)
    {
        var faker = new Faker();

        string[][] result = new string[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new string[columns];

            for (int j = 0; j < columns; j++)
            {
                result[i][j] = GetRandomDataByColumn(faker, j);
            }
        }

        return result;
    }

    private string GetRandomDataByColumn(Faker faker, int columnIndex)
    {
        return columnIndex switch
        {
            0 => faker.Name.FullName(),
            1 => faker.Internet.Email(),
            2 => faker.Phone.PhoneNumber(),
            3 => faker.Address.FullAddress(),
            4 => faker.Company.CompanyName(),
            5 => faker.Date.Recent().ToString("yyyy-MM-dd HH:mm:ss"),
            6 => faker.Commerce.ProductName(),
            7 => faker.Random.Double(1, 100).ToString("F2"),
            _ => faker.Random.AlphaNumeric(10)
        };
    }

    // Method to display sample data
    public void DisplaySample(string[][] data, int sampleSize = 5)
    {
        Console.WriteLine($"Sample of {sampleSize} rows:");
        Console.WriteLine(new string('=', 100));

        // Headers
        string[] headers = { "Name", "Email", "Phone", "Address", "Company", "Date", "Product", "Value" };
        Console.WriteLine($"{"Index",-5} {string.Join(" | ", headers)}");
        Console.WriteLine(new string('-', 100));

        for (int i = 0; i < Math.Min(sampleSize, data.Length); i++)
        {
            Console.Write($"{i,-5} ");
            for (int j = 0; j < data[i].Length; j++)
            {
                string value = data[i][j];
                if (value.Length > 20) value = value.Substring(0, 17) + "...";
                Console.Write($"{value,-20} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine(new string('=', 100));
        Console.WriteLine($"Total rows: {data.Length}");
        Console.WriteLine($"Columns per row: {(data.Length > 0 ? data[0].Length : 0)}");
    }

    // Method to save to CSV file
    public void SaveToCsv(string[][] data, string filePath)
    {
        string[] headers = { "Name", "Email", "Phone", "Address", "Company", "Date", "Product", "Value" };

        using (var writer = new System.IO.StreamWriter(filePath))
        {
            // Write headers
            writer.WriteLine(string.Join(",", headers));

            // Write data
            foreach (var row in data)
            {
                // Escape commas in values by wrapping in quotes
                var escapedRow = new string[row.Length];
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i].Contains(",") || row[i].Contains("\""))
                    {
                        escapedRow[i] = $"\"{row[i].Replace("\"", "\"\"")}\"";
                    }
                    else
                    {
                        escapedRow[i] = row[i];
                    }
                }
                writer.WriteLine(string.Join(",", escapedRow));
            }
        }

        Console.WriteLine($"Data saved to: {filePath}");
    }
}