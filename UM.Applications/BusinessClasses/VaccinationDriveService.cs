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
    public class VaccinationDriveService : IVaccinationDriveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VaccinationDriveService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public void Add(VaccinationDriveModel entity)
        {
            var result = _mapper.Map<VaccinationDrive>(entity);
            _unitOfWork.VaccinationDriveRepo.Add(result);
        }

        public VaccinationDriveModel GetDetail(BaseSearchEntity search)
        {
            BaseSpecification<VaccinationDrive> spec = new()
            {
                Criteria = a => a.UniqueId == search.PrimaryKey
            };
            return _mapper.Map<VaccinationDriveModel>(_unitOfWork.VaccinationDriveRepo.GetUniqueRecordBySpec(spec));
        }

        public PaginedList<VaccinationDriveModel> GetList(BaseSearchEntity baseSearch)
        {
            BaseSpecification<VaccinationDrive> specification = new()
            {
            };
            var result = _unitOfWork.VaccinationDriveRepo.GetListByPage(baseSearch.StartIndex, baseSearch.PageSize, null, specification);
            return result == null ? null : new PaginedList<VaccinationDriveModel> { Items = _mapper.Map<List<VaccinationDriveModel>>(result.Items), TotalItems = result.TotalItems };
        }

        public void Update(VaccinationDriveModel entity)
        {
            var result = _mapper.Map<VaccinationDrive>(entity);
            result.ModifiedDate = DateTime.Now;
            _unitOfWork.VaccinationDriveRepo.Update(result, ["UniqueId", "CreatedDate"]);
        }
    }
}
