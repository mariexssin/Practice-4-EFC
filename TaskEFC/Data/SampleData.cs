using TaskEFC.Models;

namespace TaskEFC.Data
{
    public static class SampleData
    {
        public static void Initialize(ShoppingContext context)
        {
            if (context.Customers.Any())
            {
                return; 
            }

            var customers = new Customer[]
            {
                new Customer { LastName = "Sinitsina", FirstName = "Zosya", Address = "Chernomorsk", Discount = 0 },
                new Customer { LastName = "Panikovskyi", FirstName = "Semiuel M.", Address = "Chernomorsk", Discount = 5 },
                new Customer { LastName = "Kozlevych", FirstName = "Adam", Address = "Chernomorsk", Discount = 0 },
                new Customer { LastName = "Gritsatsuieva", FirstName = "Madam", Address = "Ensk", Discount = 0 },
                new Customer { LastName = "Bender", FirstName = "Ostap", Address = "Rio de Zhmerinka", Discount = 0 },
                new Customer { LastName = "Balaganov", FirstName = "Shura", Address = "Odessa", Discount = 10 }
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var supermarkets = new SuperMarket[]
            {
                new SuperMarket { Name = "Billa", Address = "Odessa" },
                new SuperMarket { Name = "Big Pocket", Address = "Lviv" },
                new SuperMarket { Name = "Wellmart", Address = "Chernivtsi, Rus'ka" },
                new SuperMarket { Name = "Silpo", Address = "Chernivtsi, Holovna" },
                new SuperMarket { Name = "ATB", Address = "Kyiv" }
            };
            context.SuperMarkets.AddRange(supermarkets);
            context.SaveChanges();

            var products = new Product[]
            {
                new Product { Name = "Banana", Price = 45.5f },
                new Product { Name = "Butter", Price = 82.0f },
                new Product { Name = "Cola", Price = 35.0f },
                new Product { Name = "Bread", Price = 24.0f },
                new Product { Name = "Milk", Price = 42.5f }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            var order = new Order
            {
                OrderDate = new DateTime(2020, 8, 23),
                CustomerId = customers.Single(c => c.LastName == "Balaganov").Id,
                SuperMarketId = supermarkets.Single(s => s.Name == "Billa").Id
            };
            context.Orders.Add(order);
            context.SaveChanges();

            var orderDetails = new OrderDetail[]
            {
                new OrderDetail { OrderId = order.Id, ProductId = products.Single(p => p.Name == "Banana").Id, Quantity = 1 },
                new OrderDetail { OrderId = order.Id, ProductId = products.Single(p => p.Name == "Butter").Id, Quantity = 2 },
                new OrderDetail { OrderId = order.Id, ProductId = products.Single(p => p.Name == "Cola").Id, Quantity = 1 }
            };
            context.OrderDetails.AddRange(orderDetails);
            context.SaveChanges();
        }
    }
}