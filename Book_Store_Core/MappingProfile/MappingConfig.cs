using Book_Store_Core.Dto_s.InvoiceDTO_s;
using Book_Store_Core.Dto_s.CartDTO_S;
using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Dto_s.CategoryDTO_s;
using Book_Store_Core.Dto_s.Order;
using Book_Store_Core.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.MappingProfile
{
    public class MappingConfig
    {
        public class Mapping_Profile
        {
            private static readonly TypeAdapterConfig _config = new TypeAdapterConfig();
            static Mapping_Profile()
            {

                _config.NewConfig<Book, BookDTO>()
                    .Map(dest => dest.CategoryName, src => src.Category.Name); // Example of custom mapping

                _config.NewConfig<Category , CategoryDTO>()
                 .Map(dest => dest.Books, src => src.Books);


                _config.NewConfig<UpdateBookDto, Book>()
                .IgnoreNullValues(true); // Ignore null values during mapping

                _config.NewConfig<ShoppingCartItem, UserCartItemsDTO>()
                .Map(dest => dest.Price, src => src.Book.Price) // Map Price from the related Book entity
                .Map(dest => dest.BookTitle, src => src.Book.Title) // Map BookTitle from Book entity
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice) // Calculate TotalPrice
                 .Map(dest => dest.BookTitle, src => src.Book.Title); // Map Book's Title
                _config.NewConfig<Invoice, InvoiceDTO>()
            .Map(dest => dest.InvoiceItems, src => src.InvoiceItems.Select(item => item.Adapt<InvoiceItemDTO>()).ToList());
                _config.NewConfig<InvoiceDTO, Invoice>()
    .Map(dest => dest.InvoiceItems, src => src.InvoiceItems.Select(item => item.Adapt<InvoiceItem>()).ToList());
               

            }
            public static TypeAdapterConfig Config => _config;
        }
        }
}
