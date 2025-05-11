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
    public class VaccinationStudentMapperService : IVaccinationStudentMapperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VaccinationStudentMapperService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public void Add(VaccinationStudentMapperModel entity)
        {
            var result = _mapper.Map<VaccinationStudentMapper>(entity);
            _unitOfWork.VaccinationStudentMapperRepo.Add(result);
        }

        public VaccinationStudentMapperModel GetDetail(BaseSearchEntity search)
        {
            BaseSpecification<VaccinationStudentMapper> spec = new()
            {
                Criteria = a => a.UniqueId == search.PrimaryKey
            };
            return _mapper.Map<VaccinationStudentMapperModel>(_unitOfWork.VaccinationStudentMapperRepo.GetUniqueRecordBySpec(spec));
        }

        public PaginedList<VaccinationStudentMapperModel> GetList(BaseSearchEntity baseSearch)
        {
            BaseSpecification<VaccinationStudentMapper> specification = new()
            {
            };
            var result = _unitOfWork.VaccinationStudentMapperRepo.GetListByPage(baseSearch.StartIndex, baseSearch.PageSize, null, specification);
            return result == null ? null : new PaginedList<VaccinationStudentMapperModel> { Items = _mapper.Map<List<VaccinationStudentMapperModel>>(result.Items), TotalItems = result.TotalItems };
        }

        public void Update(VaccinationStudentMapperModel entity)
        {
            var result = _mapper.Map<VaccinationStudentMapper>(entity);
            _unitOfWork.VaccinationStudentMapperRepo.Update(result, ["UniqueId", "CreatedDate"]);
        }
    }
}
