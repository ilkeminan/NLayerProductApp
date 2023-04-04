using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    public class CategorySeeds : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Telefon"},
                new Category { Id = 2, Name = "Bilgisayar" },
                new Category { Id = 3, Name = "Elektronik" });
        }
    }
}
