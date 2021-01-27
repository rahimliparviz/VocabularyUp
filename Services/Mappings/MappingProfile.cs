using AutoMapper;
using Domain;
using Services.DTO;

namespace Services.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Language, LanguageGetDto>();
            CreateMap<Phrase, PhraseGetDto>();
            CreateMap<Translation, TranslationGetDto>();
            CreateMap<TranslationPostDto, Translation>();
             CreateMap<Translation, TranslationPostDto>();
            CreateMap<Phrase, PhrasesWithTranslationsDto>()
                ;

            // CreateMap<Expense, ExpenseDto>();
            //
            //
            // CreateMap<Customer, CustomerDto>();
            // CreateMap<Supplier, SupplierDto>();
            // CreateMap<Order, OrderDto>()
            //     .ForMember(e => e.CustomerName,
            //         o => o.MapFrom(x => x.Customer.Name))
            //     .ForMember(e => e.CustomerAddress,
            //         o => o.MapFrom(x => x.Customer.Address))
            //     .ForMember(e => e.CustomerPhone,
            //         o => o.MapFrom(x => x.Customer.Phone));
            
            
            
        }
    }
}