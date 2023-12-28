using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;

    public TransactionController(IMapper mapper, ITransactionService transactionService)
    {
        _mapper = mapper;
        _transactionService = transactionService;
    }

    [HttpGet("period/name/{name}")]
    public IActionResult ListAsync(string name)
    {
        var transactions = _transactionService.ListByPeriod(name)
                            .OrderBy(x => x.InvoiceDate).ThenByDescending(x => (double)x.InvoiceValue);

        var balance = _transactionService.GetBalance();
        return Ok(new ListTransactionsResponse
        {
            Balance = balance,
            Transactions = transactions.Select(_mapper.Map<ListTransactionsNodeResponse>)
        });
    }

    [HttpGet("period/{id}")]
    public IActionResult ListAsync(Guid id)
    {
        var transactions = _transactionService.ListByPeriod(id)
                            .OrderBy(x => x.InvoiceDate).ThenByDescending(x => (double)x.InvoiceValue);

        var balance = _transactionService.GetBalance();
        return Ok(new ListTransactionsResponse
        {
            Balance = balance,
            Transactions = transactions.Select(_mapper.Map<ListTransactionsNodeResponse>)
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AddTransactionRequest request)
    {
        var newTransaction = await _transactionService.CreateAsync(_mapper.Map<Transaction>(request));
        var response = _mapper.Map<AddTransactionResponse>(newTransaction);
        return Created(Request.Path, response);
    }


}
