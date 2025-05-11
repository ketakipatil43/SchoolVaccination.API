using AutoMapper;
using DataHelper.Entities;
using DataHelper.HelperClasses;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;
using UM.Data.UnitOfWork.Interfaces;

namespace UM.Applications.BusinessClasses
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public PaginedList<DashboardModel> GetList(BaseSearchEntity baseSearch)
        {
            BaseSpecification<VaccinationDrive> specification = new()
            {
                Includes = [a => a.VaccinationStudentMapper]
            };
            var result = _unitOfWork.VaccinationDriveRepo.GetListByPage(baseSearch.StartIndex, baseSearch.PageSize, null, specification);
            return result == null ? null : new PaginedList<DashboardModel> { Items = _mapper.Map<List<DashboardModel>>(result.Items), TotalItems = result.TotalItems };
        }
    }
}
