using Microsoft.AspNetCore.Mvc;
using UM.API.Utilities;
using UM.Core.BaseEntities;
using UM.Core.Common;

namespace UM.API.Controllers.Common
{
    /// <summary>
    /// Generic Controller class to perform Insert Update and Delete Operations
    /// It takes two Generic Types as M and S
    /// </summary>
    /// <typeparam name="M">M is of Type BaseModelEntity or should be inherited from BaseModelEntity with Parameter Less constructor</typeparam>
    /// <typeparam name="S">S is of Type  BaseSearchEntity or should be inherited from BaseSearchEntity with Parameter Less constructor</typeparam>
    /// <returns></returns>
    public abstract class PageController<M, S> : SearchController<M, S> where M : BaseModelEntity, new() where S : BaseSearchEntity
    {
        protected PageController() { }


        /// <summary>
        /// It Insert ot Update an exisitng record based on Flag Property value either Add (1) or Edit (0)
        /// </summary>
        /// <param name="formData"></param>
        /// <returns> MainViewModel<M></returns>
        [HttpPost("insertupdate")]
        public virtual MainViewModel<M> InsertUpdate([FromBody] M formData)
        {
            int action = formData.Flag;
            if (action == ActionFlags.Add)
                InternalUpdateNewDetails(formData);
            else
                InternalUpdateExistingDetails(formData);

            return new MainViewModel<M>();
        }

        /// <summary>
        /// It Deletes the Selected record
        /// </summary>
        /// <param name="formData"></param>
        /// <returns>MainViewModel<M></returns>
        [HttpPost("delete")]
        public virtual MainViewModel<M> DeleteEntity([FromBody] M formData)
        {
            InternalDeleteDetails(formData);
            return new MainViewModel<M>();
        }


        [HttpPost("deletebyids")]
        public virtual MainViewModel<M> DeleteEntity([FromBody] List<M> formData)
        {

            InternalDeleteByIds(formData);
            return new MainViewModel<M>();
        }

        /// <summary>
        ///  Must override in parent controller to save the New request object
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="message"></param>
        protected virtual void InternalUpdateNewDetails(M newEntity)
        {
        }

        /// <summary>
        /// Must override in parent controller to update the existing request object
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <param name="message"></param>
        protected virtual void InternalUpdateExistingDetails(M updatedEntity)
        {
        }

        /// <summary>
        /// Must override in parent controller to delete the existing request object
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="message"></param>
        protected virtual void InternalDeleteDetails(M deleteEntity)
        {
        }

        protected virtual void InternalDeleteByIds(List<M> entityList)
        {
        }

    }
}
