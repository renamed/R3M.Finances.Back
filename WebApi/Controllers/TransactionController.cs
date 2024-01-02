using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Model;
using WebApi.Services;
using System.Net.Http;

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
    public async Task<IActionResult> ListAsync(string name)
    {
        var transactions = (await _transactionService.ListByPeriodAsync(name))
                            .OrderBy(x => x.InvoiceDate).ThenByDescending(x => (double)x.InvoiceValue);

        var balance = _transactionService.GetBalance();
        return Ok(new ListTransactionsResponse
        {
            Balance = balance,
            Transactions = transactions.Select(_mapper.Map<ListTransactionsNodeResponse>)
        });
    }

    [HttpGet("period/{id}")]
    public async Task<IActionResult> ListAsync(Guid id)
    {
        var transactions = (await _transactionService.ListByPeriodAsync(id))
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
        //try
        //{
        //    try
        //    {
                var newTransaction = await _transactionService.CreateAsync(_mapper.Map<Transaction>(request));
                var response = _mapper.Map<AddTransactionResponse>(newTransaction);
                return Created(Request.Path, response);
        //    }
        //    catch (ServiceException e)
        //    {

        //        //var responseBody = JsonSerializer.Serialize(new ErrorDto { Message = e.Message });

        //        //HttpContext.Response.StatusCode = (int)e.GetStatusCode();
        //        //HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

        //        //await HttpContext.Response.WriteAsync(responseBody, Encoding.UTF8);

        //        return StatusCode((int)e.GetStatusCode(), new ErrorDto { Message = e.Message });
        //    }

        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}
        //catch (Exception e)
        //{
        //    return Ok();
        //}

    }
}
