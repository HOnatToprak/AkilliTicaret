using Microsoft.EntityFrameworkCore;
namespace AkilliTicaret.Quiz;

public class Quiz
{
    AkilliTicaretDbContext _dbContext;
    public Quiz(AkilliTicaretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public OrderStatistics GetOrderStatistics(List<Order> orders)
    {
        // Burayı doldurun:
        // Verilen siparişlerin içinde, her bir kategoride toplam
        // kaç ürün satıldığını ve toplam ne kadarlık ürün
        // satıldığını hesaplayan kodu yazınız.

        // Getting prices from OrderProduct for using ProductId and OrderId
        List<Entity.OrderProduct> orderProducts = new();
        foreach (Order order in orders)
        {
            List<int> productIds = order.products
                .Select(product => product.ID)
                .ToList();

            List<Entity.OrderProduct> orderProductGroup = _dbContext.OrderProducts
                .Where(op =>
                    op.OrderId == order.ID
                    && productIds.Contains(op.ProductId))
                // for getting category
                .Include(op => op.Product)
                .ToList();

            orderProducts.AddRange(orderProductGroup);
        }

        // Group orderproducts by category
        // and calculate statistics for categories individually.
        var orderStatisticDictionary =
            orderProducts
            .GroupBy(op => op.Product.CategoryId)
            .ToDictionary(group => group.Key, group => new OrderStatisticCategory()
            {
                NumberOfProductsSold = group.Count(),
                TotalPriceOfProductsSold = Decimal.ToDouble(group.Sum(op => op.Price))
            });


        // Add statistic of subcategories to parent category statistics incrementally.
        foreach (var statistic in orderStatisticDictionary)
        {
            int? parent = _dbContext.Categories.Find(statistic.Key)!.ParentId;
            while (parent is not null
                    && orderStatisticDictionary.ContainsKey(parent.Value))
            {
                orderStatisticDictionary[parent.Value].NumberOfProductsSold
                    += statistic.Value.NumberOfProductsSold;

                orderStatisticDictionary[parent.Value].TotalPriceOfProductsSold
                    += statistic.Value.TotalPriceOfProductsSold;

                parent = _dbContext.Categories.Find(parent.Value)!.ParentId;
            }
        }

        return new OrderStatistics()
        {
            categories =
                orderStatisticDictionary.Select(pair => pair.Value).ToList(),
        };
    }

    public List<Product> GetProductsOfCategoryAndDescendants(int categoryID)
    {
        // Burayı doldurun:
        // Verilen ID'ye sahip kategori ve onun alt kategorilerindeki
        // tüm ürünleri veritabanından alıp List<Product>
        // tipinde döndüren kodu yazınız.

        // Populating subcategories and subcategory of subcategories ...
        List<int> categories = new();
        PopulateSubcategoriesRecursive(categoryID, categories);

        // Getting products of category list
        List<Entity.Product> products = _dbContext.Products
          .Where(product => categories.Contains(product.CategoryId))
          .ToList();

        // Transforming to expected type
        return products.Select(product => new Product()
        {
            ID = product.Id,
            CategoryID = product.CategoryId,
        }).ToList();
    }

    private void PopulateSubcategoriesRecursive(int categoryId, List<int> accumulator)
    {
        accumulator.Add(categoryId);

        List<int> children = _dbContext.Categories
          .Where(category => category.ParentId == categoryId)
          .Select(category => category.Id)
          .ToList();

        foreach (int child in children)
        {
            // Circle references should be checked when adding new category.
            // Activate code below, if there are circle references.
            // If code below is activated, consider dictionary for faster lookups.
            // if(accumulator.Contains(child))
            //   return;

            PopulateSubcategoriesRecursive(child, accumulator);
        }
    }
}

public class Product
{
    public int ID { get; set; }
    public int CategoryID { get; set; }
}

public class Order
{
    // I tought ID was missing.
    public int ID { get; set; }
    public List<Product> products { get; set; } = default!;
}

public class OrderStatistics
{
    public List<OrderStatisticCategory> categories { get; set; } = default!;
}

public class OrderStatisticCategory
{
    public int NumberOfProductsSold { get; set; }
    public double TotalPriceOfProductsSold { get; set; }
}
