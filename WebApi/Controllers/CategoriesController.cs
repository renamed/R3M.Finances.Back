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
        // Inicialize o dicionário para armazenar as categorias
        Dictionary<Guid, ListCategoriesResponse> response = new Dictionary<Guid, ListCategoriesResponse>();

        // Mapeie as categorias para o dicionário e crie os nós principais
        foreach (var category in categories)
        {
            // Verifique se a categoria já está no dicionário
            if (!response.ContainsKey(category.Id))
            {
                response[category.Id] = _mapper.Map<ListCategoriesResponse>(category);
            }

            // Verifique se a categoria tem um pai e adicione-a como filho, se aplicável
            if (category.ParentId.HasValue && response.ContainsKey(category.ParentId.Value))
            {
                response[category.ParentId.Value].Children.Add(response[category.Id]);
            }
        }

        // Encontre as raízes (categorias sem pais) e retorne como resultado
        List<ListCategoriesResponse> roots = new();
        foreach (var category in response.Values)
        {
            if (!categories.Any(c => c.Id == category.Id && c.ParentId.HasValue))
            {
                roots.Add(category);
            }
        }

        return roots;
    }
}
