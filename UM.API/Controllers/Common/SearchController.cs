using DataHelper.Entities;
using Microsoft.AspNetCore.Mvc;
using UM.Core.BaseEntities;
using UM.Core.Common;

namespace UM.API.Controllers.Common
{
    /// <summary>
    /// Generic Controller class to perform Search with Single or List of Records
    /// It takes two Generic Types as M and S
    /// </summary>
    /// <typeparam name="M">M is of Type Class with Parameter Less constructor</typeparam>
    /// <typeparam name="S">S is of Type  BaseSearchEntity or should be inherited from BaseSearchEntity with Parameter Less constructor</typeparam>
    /// <returns></returns>
    public class SearchController<M, S> : BaseController where M : class, new() where S : BaseSearchEntity
    {
        public SearchController() { }
        /// <summary>
        /// It returns a single record of Type M wrapped with MainViewModel which has
        /// Two parameters as T and Message
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>MainViewModel<M></returns>
        [HttpPost("search")]
        public virtual MainViewModel<M> GetSingle([FromBody] S formData)
        {
            M results = new();
            results = GetSingleRecord(formData);
            return new MainViewModel<M>(results);
        }

        /// <summary>
        /// It returns a list of records of Type M wrapped with MainViewModel which has
        /// Two parameters as T and Message
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public MainViewModel<PaginedList<M>> List([FromQuery] S formData)
        {
            PaginedList<M> result = null;
            result = GetList(formData);
            return new MainViewModel<PaginedList<M>>(result);
        }

        /// <summary>
        /// Must override in parent controller to get the actual result to be sent to the response
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns>List<M></returns>
        protected virtual PaginedList<M> GetList(S searchModel)
        {
            return new PaginedList<M>();
        }


        /// <summary>
        /// Must override in parent controller to get the actual result to be sent to the response
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns>M</returns>
        protected virtual M GetSingleRecord(S searchModel)
        {
            return new M();
        }
    }
}
