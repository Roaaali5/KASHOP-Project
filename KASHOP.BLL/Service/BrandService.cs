using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;

        public BrandService(IBrandRepository brandRepository, IFileService fileService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
        }

        public async Task CreateBrand(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();
            if (request.Logo != null)
            {
                var imagePath = await _fileService.UploadAsync(request.Logo);
                brand.Logo = imagePath;
            }
            await _brandRepository.CareateAsync(brand);

        }


        public async Task<List<BrandResponse>> GetAllBrandsAsynce()
        {
            var brands = await _brandRepository.GetAllAsync(
                new string[]
                {
                    nameof(Brand.Translations),
                    nameof(Brand.CreatedBy)
                });
            return brands.Adapt<List<BrandResponse>>();
        }



        public async Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _brandRepository.GetOne(filter, new string[]
                {
                    nameof(Brand.Translations),
                    nameof(Brand.CreatedBy)
                });
            if (brand == null) return null;
            return brand.Adapt<BrandResponse>();
        }


        public async Task<bool> DeleteBrand(int id)
        {
            var brand = await _brandRepository.GetOne(c => c.Id == id);
            if (brand == null) return false;

            _fileService.DeleteAsynce(brand.Logo);
            return await _brandRepository.DeleteAsync(brand);
        }
    }
}
