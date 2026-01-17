using web_api_p5.Models;

namespace web_api_p5.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any customers.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            var customers = new Customer[]
            {
                new Customer{Email="admin@example.com", Password="adminpassword", FullName="Admin User", PhoneNumber="0000000000", Address="Admin Address", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
                new Customer{Email="customer1@example.com", Password="password123", FullName="Nguyen Van A", PhoneNumber="0123456789", Address="123 Le Loi", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
                new Customer{Email="customer2@example.com", Password="password123", FullName="Tran Thi B", PhoneNumber="0987654321", Address="456 Nguyen Hue", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
                new Customer{Email="customer3@example.com", Password="password123", FullName="Le Van C", PhoneNumber="0911223344", Address="789 Hai Ba Trung", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
                new Customer{Email="customer4@example.com", Password="password123", FullName="Pham Thi D", PhoneNumber="0955667788", Address="101 Dien Bien Phu", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow}
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();

            var menuItems = new MenuItem[]
            {
                new MenuItem{Name="Pho Bo", Description="Traditional Beef Noodle Soup", Category="Main Course", Price=80000, PreparationTime=15, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Bun Cha", Description="Grilled Pork with Rice Noodles", Category="Main Course", Price=75000, PreparationTime=20, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Goi Cuon", Description="Fresh Spring Rolls", Category="Appetizer", Price=50000, PreparationTime=10, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Cha Gio", Description="Fried Spring Rolls", Category="Appetizer", Price=55000, PreparationTime=15, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Com Tam", Description="Broken Rice with Pork Chop", Category="Main Course", Price=65000, PreparationTime=15, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Ca Phe Sua Da", Description="Iced Milk Coffee", Category="Beverage", Price=35000, PreparationTime=5, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Che Ba Mau", Description="Three Color Dessert", Category="Dessert", Price=30000, PreparationTime=5, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Canh Chua", Description="Sour Soup", Category="Soup", Price=60000, PreparationTime=15, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Rau Muong Xao", Description="Stir-fried Morning Glory", Category="Appetizer", Price=40000, PreparationTime=10, IsVegetarian=true, IsAvailable=true, UpdatedAt=DateTime.UtcNow},
                new MenuItem{Name="Bun Bo Hue", Description="Spicy Beef Noodle Soup", Category="Main Course", Price=85000, PreparationTime=20, IsSpicy=true, IsAvailable=true, UpdatedAt=DateTime.UtcNow}
            };
            foreach (MenuItem m in menuItems)
            {
                context.MenuItems.Add(m);
            }
            context.SaveChanges();

            var tables = new TableEntity[]
            {
                new TableEntity{TableNumber="T01", Capacity=2, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T02", Capacity=2, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T03", Capacity=4, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T04", Capacity=4, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T05", Capacity=6, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T06", Capacity=8, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T07", Capacity=10, IsAvailable=true, CreatedAt=DateTime.UtcNow},
                new TableEntity{TableNumber="T08", Capacity=2, IsAvailable=true, CreatedAt=DateTime.UtcNow}
            };
            foreach (TableEntity t in tables)
            {
                context.Tables.Add(t);
            }
            context.SaveChanges();
        }
    }
}
