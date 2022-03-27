using NUnit.Framework;

namespace Quotes.Tests;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Features;
using MediatR;
using Moq;

public class UpdateTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task UpdateContentTest()
    {
        var dbContext = Data.TestDbContext();

        string updatedQuote = "test update";

        var cmd = new Update.Command
        {
            Id = Data.AlbertEinsteinInfiniteQuoteId,
            AuthorId = Data.AlbertEinsteinId,
            Contents = updatedQuote
        };

        var handler = new Update.Command.CommandHandler(dbContext, new Mock<IMediator>().Object);

        await handler.Handle(cmd, CancellationToken.None);

        Quote? updated = dbContext.Quotes.FirstOrDefault(q => q.Id == Data.AlbertEinsteinInfiniteQuoteId);
        
        Assert.IsNotNull(updated);
        
        Assert.AreEqual(updatedQuote, updated?.Contents);
    }
}