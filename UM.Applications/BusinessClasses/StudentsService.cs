using AutoMapper;
using DataHelper.Entities;
using DataHelper.HelperClasses;
using UM.Applications.BusinessInterfaces;
using UM.Core.BaseEntities;
using UM.Core.DBEntities;
using UM.Core.ModelEntities;
using UM.Core.SearchEntities;
using UM.Data.UnitOfWork.Interfaces;

namespace UM.Applications.BusinessClasses
{
    public class StudentsService : IStudentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentsService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }
        public void Add(StudentsModel entity)
        {
            var result = _mapper.Map<Students>(entity);
            _unitOfWork.StudentsRepo.Add(result);
        }

        public StudentsModel GetDetail(StudentSearchModel search)
        {
            BaseSpecification<Students> spec = new()
            {
                Criteria = a => a.UniqueId == search.PrimaryKey
            };
            return _mapper.Map<StudentsModel>(_unitOfWork.StudentsRepo.GetUniqueRecordBySpec(spec));
        }

        public PaginedList<StudentsModel> GetList(StudentSearchModel baseSearch)
        {
            BaseSpecification<Students> baseSpecification = new();
            if (baseSearch.IsFilterApplied)
            {
                if (baseSearch.StudentId > 0)
                {
                    baseSpecification.Criteria = a => a.UniqueId == baseSearch.StudentId;
                }
                if (baseSearch.VaccinationDriveId > 0)
                {
                    baseSpecification.Criteria = a =>
                            a.VaccinationStudentMapper.Select(m => m.VaccinationDriveUniuqId).Contains(baseSearch.VaccinationDriveId);
                    baseSpecification.Includes = [a => a.VaccinationStudentMapper];
                }
                else
                {
                    baseSpecification.Includes = [a => a.VaccinationStudentMapper];
                }
            }
            var result = _unitOfWork.StudentsRepo.GetListByPage(baseSearch.StartIndex, baseSearch.PageSize, null, baseSpecification);
            return result == null ? null : new PaginedList<StudentsModel> { Items = _mapper.Map<List<StudentsModel>>(result.Items), TotalItems = result.TotalItems };
        }

        public void Update(StudentsModel entity)
        {
            var result = _mapper.Map<Students>(entity);
            result.ModifiedDate = DateTime.Now;
            _unitOfWork.StudentsRepo.Update(result, ["UniqueId", "CreatedDate"]);
        }
    }
}
