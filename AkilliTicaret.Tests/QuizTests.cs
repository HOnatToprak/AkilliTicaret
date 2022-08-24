using Microsoft.EntityFrameworkCore;
using AkilliTicaret.Quiz;

namespace AkilliTicaret.Tests;

[Collection("Sequential")]
public class QuizTests : IClassFixture<DatabaseFixture>
{
    public DatabaseFixture Fixture { get; }
    public QuizTests(DatabaseFixture fixture)
    {
      Fixture = fixture;
    }

    [Fact]
    public void GetOrderStatisticsTest()
    {
        Quiz.Quiz quiz = new(Fixture.DbContext);
        List<Order> orders = new();

        for(int i = 0; i < 4; ++i)
        {
            List<Product> products = new();
            for(int j = 0; j < 4; ++j)
            {
                products.Add(new()
                {
                    ID = j + 1,
                    CategoryID = j + 1,
                });
            }
            orders.Add(new()
            {
                ID = i + 1,
                products = products,
            }); 
        }

        OrderStatistics result = quiz.GetOrderStatistics(orders);
        Assert.Equal(16, result.categories[0].NumberOfProductsSold);
        Assert.Equal(280, result.categories[0].TotalPriceOfProductsSold);
        Assert.Equal(8, result.categories[1].NumberOfProductsSold);
        Assert.Equal(144, result.categories[1].TotalPriceOfProductsSold);
        Assert.Equal(4, result.categories[2].NumberOfProductsSold);
        Assert.Equal(72, result.categories[2].TotalPriceOfProductsSold);
        Assert.Equal(4, result.categories[3].NumberOfProductsSold);
        Assert.Equal(76, result.categories[3].TotalPriceOfProductsSold);

    }


    [Fact]
    public void GetProductsOfCategoryAndDescendantsTest()
    {
        Quiz.Quiz quiz = new(Fixture.DbContext);
        var category1 = quiz.GetProductsOfCategoryAndDescendants(1);
        var category2 = quiz.GetProductsOfCategoryAndDescendants(2);
        var category3 = quiz.GetProductsOfCategoryAndDescendants(3);
        var category4 = quiz.GetProductsOfCategoryAndDescendants(4);


        int[] category1Ids = category1.Select(p => p.ID).ToArray();
        Assert.Contains(1, category1Ids);
        Assert.Contains(2, category1Ids);
        Assert.Contains(3, category1Ids);
        Assert.Contains(4, category1Ids);
        Assert.Equal(4, category1Ids.Length);

        int[] category2Ids = category2.Select(p => p.ID).ToArray();
        Assert.Contains(2, category2Ids);
        Assert.Contains(4, category2Ids);
        Assert.Equal(2, category2Ids.Length);

        int[] category3Ids = category3.Select(p => p.ID).ToArray();
        Assert.Contains(3, category3Ids);
        Assert.Single(category3Ids);

        int[] category4Ids = category4.Select(p => p.ID).ToArray();
        Assert.Contains(4, category4Ids);
        Assert.Single(category4Ids);
    }
}

public class DatabaseFixture : IDisposable
{
    public AkilliTicaretDbContext DbContext { get; }
    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AkilliTicaretDbContext>();
        options.UseSqlite("Data Source=mydb.db");

        DbContext = new AkilliTicaretDbContext(options.Options);
        DbContext.Database.EnsureCreated();

        Entity.Category category1 = new()
        {
            Id = 1,
            Parent = null,
        };
        
        Entity.Category category2 = new()
        {
            Id = 2,
            Parent = category1,
        };

        Entity.Category category3 = new()
        {
            Id = 3,
            Parent = category1,
        };

        Entity.Category category4 = new()
        {
            Id = 4,
            Parent = category2,
        };

        DbContext.Categories.Add(category1);
        DbContext.Categories.Add(category2);
        DbContext.Categories.Add(category3);
        DbContext.Categories.Add(category4);

        Entity.Product product1 = new()
        {
            Id = 1,
            Category = category1,
        };

        Entity.Product product2 = new()
        {
            Id = 2,
            Category = category2,
        };

        Entity.Product product3 = new()
        {
            Id = 3,
            Category = category3,
        };

        Entity.Product product4 = new()
        {
            Id = 4,
            Category = category4,
        };

        DbContext.Products.AddRange(product1, product2, product3, product4);

        Entity.Order order1 = new()
        {
            Id = 1,
            CustomerId = 1,
            Price = 35,
        };

        Entity.Order order2 = new()
        {
            Id = 2,
            CustomerId = 2,
            Price = 42,
        };

        Entity.Order order3 = new()
        {
            Id = 3,
            CustomerId = 3,
            Price = 51,
        };

        Entity.Order order4 = new()
        {
            Id = 4,
            CustomerId = 4,
            Price = 51,
        };

        DbContext.Orders.AddRange(order1, order2, order3, order4);

        List<Entity.OrderProduct> ops = new();
        for(int i = 0; i < 16; ++i)
        {
            ops.Add(new()
            {
                Id = i + 1,
                OrderId = (i / 4) + 1,
                ProductId = (i % 4) + 1,
                Price = 10 + i,
            }) ;
        }

        DbContext.OrderProducts.AddRange(ops);


        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}
