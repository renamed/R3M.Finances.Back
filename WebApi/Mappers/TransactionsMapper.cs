using AutoMapper;
using WebApi.Dtos;
using WebApi.Model;

namespace WebApi.Mappers;

public class TransactionsMapper : Profile
{
    public TransactionsMapper()
    {
        CreateMap<Transaction, ListTransactionsNodeResponse>();
        CreateMap<AddTransactionRequest, Transaction>();
        CreateMap<AddTransactionPartRequest, TransactionPart>();
        CreateMap<Transaction, AddTransactionResponse>();
        CreateMap<TransactionPart, AddTransactionPartResponse>();
    }
}
