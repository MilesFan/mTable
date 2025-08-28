using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

public class BookGenerator
{
    public List<Book> GenerateBooks(int count = 1000)
    {
        // Define genres
        var genres = new[] { "Fiction", "Non-Fiction", "Science Fiction", "Fantasy",
                           "Mystery", "Thriller", "Romance", "Biography", "History",
                           "Science", "Technology", "Self-Help", "Cooking", "Travel" };

        // Define publishers
        var publishers = new[] { "Penguin Random House", "HarperCollins", "Simon & Schuster",
                               "Macmillan", "Hachette Book Group", "Scholastic", "Wiley",
                               "Oxford University Press", "Cambridge University Press" };

        // Define languages
        var languages = new[] { "English", "Spanish", "French", "German", "Italian",
                              "Portuguese", "Russian", "Chinese", "Japanese" };

        var bookFaker = new Faker<Book>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 6))
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.ISBN, f => f.Random.Replace("978-###-#####-###-#"))
            .RuleFor(b => b.PublishedDate, f => f.Date.Between(new DateTime(1950, 1, 1), DateTime.Now))
            .RuleFor(b => b.Genre, f => f.PickRandom(genres))
            .RuleFor(b => b.PageCount, f => f.Random.Int(50, 1000))
            .RuleFor(b => b.Publisher, f => f.PickRandom(publishers))
            .RuleFor(b => b.Price, f => f.Random.Decimal(5.99m, 49.99m))
            .RuleFor(b => b.IsAvailable, f => f.Random.Bool(0.8f)) // 80% available
            .RuleFor(b => b.Language, f => f.PickRandom(languages))
            .RuleFor(b => b.Rating, f => Math.Round(f.Random.Double(1, 5), 1))
            .RuleFor(b => b.Description, f => f.Lorem.Paragraphs(1, 3))
            .RuleFor(b => b.CoverImageUrl, f => f.Image.PicsumUrl());

        return bookFaker.Generate(count);
    }

    public void DisplaySampleBooks(List<Book> books, int sampleSize = 5)
    {
        Console.WriteLine($"Sample of {sampleSize} generated books:");
        Console.WriteLine("==========================================");

        foreach (var book in books.Take(sampleSize))
        {
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author}");
            Console.WriteLine($"Genre: {book.Genre}");
            Console.WriteLine($"Published: {book.PublishedDate:yyyy-MM-dd}");
            Console.WriteLine($"Pages: {book.PageCount}");
            Console.WriteLine($"Price: ${book.Price}");
            Console.WriteLine($"Rating: {book.Rating}/5");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine("------------------------------------------");
        }
    }

    public void DisplayStatistics(List<Book> books)
    {
        Console.WriteLine("\nBook Generation Statistics:");
        Console.WriteLine("===========================");
        Console.WriteLine($"Total books generated: {books.Count}");
        Console.WriteLine($"Average page count: {books.Average(b => b.PageCount):F0}");
        Console.WriteLine($"Average price: ${books.Average(b => b.Price):F2}");
        Console.WriteLine($"Average rating: {books.Average(b => b.Rating):F1}/5");
        Console.WriteLine($"Available books: {books.Count(b => b.IsAvailable)}");
        Console.WriteLine($"Out of stock: {books.Count(b => !b.IsAvailable)}");

        Console.WriteLine("\nBooks by genre:");
        var genreCounts = books.GroupBy(b => b.Genre)
                              .OrderByDescending(g => g.Count());

        foreach (var group in genreCounts)
        {
            Console.WriteLine($"{group.Key}: {group.Count()} books");
        }
    }
}