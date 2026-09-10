using AutoMapper;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Infrastructure.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Profiles
{
    public class PictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly UrlSettings _urlSettings;
        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSettings = options.Value;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var pictureUrl = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{pictureUrl}";
        }

    }
}

public class UrlSettings
{
    public string BaseUrl { get; set; }
}
