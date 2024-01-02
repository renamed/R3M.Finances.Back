using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Dtos;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{    
    private readonly IMapper _mapper;
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(IMapper mapper, ICategoriesService categoriesService)
    {
        _mapper = mapper;
        _categoriesService = categoriesService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesResponse[]), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ListAsync()
    {
        var categories = await _categoriesService.ListAsync();
        var response = BuildListCategoriesResponse(categories);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ListCategoriesResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var category = await _categoriesService.GetAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<ListCategoriesResponse>(category);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddCategoryResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AddAsync([FromBody] AddCategoryRequest addCategoryRequest)
    {
        var category = await _categoriesService.AddAsync(_mapper.Map<Category>(addCategoryRequest));
        var newCategory = _mapper.Map<AddCategoryResponse>(category);
        return Created(Request.Path, newCategory);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EditCategoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> EditAsync([FromRoute]Guid id, [FromBody] EditCategoryRequest editCategoryRequest)
    {
        var category = await _categoriesService.EditAsync(id, _mapper.Map<Category>(editCategoryRequest));
        var response = _mapper.Map<EditCategoryResponse>(category);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _categoriesService.RemoveAsync(id);
        return Ok();
    }

    private List<ListCategoriesResponse> BuildListCategoriesResponse(List<Category> categories)
    {
        var categoryMap = categories.ToDictionary(c => c.Id, _mapper.Map<ListCategoriesResponse>);

        foreach(var category in categories) 
        {
            if (category.ParentId.HasValue)
            {
                categoryMap[category.ParentId.Value].Children.Add(categoryMap[category.Id]);
            }
        }

        var response = new List<ListCategoriesResponse>();
        foreach (var category in categories.Where(x => !x.ParentId.HasValue))
        {
            response.AddRange(categoryMap.Values.Where(x => x.Id == category.Id));
        }

        return response;
        
    }
}
