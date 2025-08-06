using System;
namespace Recipify.Tests.TestsHelpers;
public static class DbSetMocking
{

    public static DbSet<T> ReturnsDbSet<T>(this Mock<ApplicationDbContext> context, IQueryable<T> data) where T : class
    {
        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        context.Setup(c => c.Set<T>()).Returns(dbSet.Object);

        if (typeof(T) == typeof(Recipe))
            context.Setup(c => c.Recipes).Returns(dbSet.Object as DbSet<Recipe>);
        if (typeof(T) == typeof(Category))
            context.Setup(c => c.Categories).Returns(dbSet.Object as DbSet<Category>);
        if (typeof(T) == typeof(Ingredient))
            context.Setup(c => c.Ingredients).Returns(dbSet.Object as DbSet<Ingredient>);

        return dbSet.Object;
    }
}
