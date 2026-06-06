using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FlashOffer-API.Application.Common.Interfaces;
using FlashOffer-API.Application.DTOs;
using FlashOffer-API.Domain.Entities;
using FlashOffer-API.Application.Common.Models;
using FlashOffer-API.Domain.Exceptions;

namespace FlashOffer-API.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/sample")]
public class SampleController : CrudControllerBase<Product, ProductDto, CreateProductDto, UpdateProductDto>
{
    public SampleController(IRepository<Product> repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
