namespace Quotes.Tests;

using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;

public static class Data
{
    public const int FrankZappaId = 1;
    public const int AlbertEinsteinId = 2;
    public const int FrankZappaBooksQuoteId = 1;
    public const int AlbertEinsteinInfiniteQuoteId = 2;

    public static readonly Quote FrankZappaBooksQuote = new()
    {
        Id = FrankZappaBooksQuoteId,
        Contents = "So many books, so little time."
    };

    public static readonly Quote AlbertEinsteinInfiniteQuote = new()
    {
        Id = AlbertEinsteinInfiniteQuoteId,
        Contents = "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe."
    };

    public static readonly Person FrankZappa = new Person
    {
        Id = FrankZappaId,
        FirstName = "Frank",
        LastName = "Zappa",
        Quotes = new List<Quote>
        {
            FrankZappaBooksQuote
        }
    };
    
    public static readonly Person AlbertEinstein = new Person
    {
        Id = AlbertEinsteinId,
        FirstName = "Albert",
        LastName = "Einstein",
        Quotes = new List<Quote>
        {
            AlbertEinsteinInfiniteQuote
        }
    };

    public static QuotesDbContext TestDbContext()
    {
        var contextOptions = new DbContextOptionsBuilder<QuotesDbContext>()
            .UseInMemoryDatabase("QuotesTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new QuotesDbContext(contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AddRange(FrankZappa, AlbertEinstein);

        context.SaveChanges();

        return context;

    }
}