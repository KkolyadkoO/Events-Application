using EventApp.Application;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.UseCases.Category;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AddCategoryUseCase _addCategoryUseCase;
    private readonly DeleteCategoryUseCase _deleteCategoryUseCase;
    private readonly GetAllCategoriesUseCase _getAllCategoriesUseCase;
    private readonly GetCategoryByIdUseCase _getCategoryByIdUseCase;
    private readonly UpdateCategoryUseCase _updateCategoryUseCase;

    public CategoriesController(
        AddCategoryUseCase addCategoryUseCase,
        DeleteCategoryUseCase deleteCategoryUseCase,
        GetAllCategoriesUseCase getAllCategoriesUseCase,
        GetCategoryByIdUseCase getCategoryByIdUseCase,
        UpdateCategoryUseCase updateCategoryUseCase)
    {
        _addCategoryUseCase = addCategoryUseCase;
        _deleteCategoryUseCase = deleteCategoryUseCase;
        _getAllCategoriesUseCase = getAllCategoriesUseCase;
        _getCategoryByIdUseCase = getCategoryByIdUseCase;
        _updateCategoryUseCase = updateCategoryUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _getAllCategoriesUseCase.Execute();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        try
        {
            var category = await _getCategoryByIdUseCase.Execute(id);
            return Ok(category);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryOfEventsRequestDto requestDto)
    {
        try
        {
            var id = await _addCategoryUseCase.Execute(requestDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, id);
        }
        catch (DuplicateCategory ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryOfEventsRequestDto requestDto)
    {
        try
        {
            await _updateCategoryUseCase.Execute(id, requestDto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await _deleteCategoryUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}