using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using DataHelper.Entities;
using DataHelper.Entities.EnumFields;
using DataHelper.HelperClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DataHelper.EFData.Common.Interfaces
{
    public interface IGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {
        public M _dbContext { get; }
        #region search methods
        /// <summary>
        /// Use this method to get all columns single row return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public T GetUniqueRecordBySpec(ISpecification<T> spec, bool asNoTracking = true)
        {
            T Entity;
            if (asNoTracking)
                Entity = ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsyncExtension().Result;
            else
                Entity = ApplySpecification(spec).FirstOrDefaultAsyncExtension().Result;
            return Entity;
        }

        /// <summary>
        /// Use this method to get all columns all rows return without any specification criteria.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> ListAll(bool isNoLock = true)
        {
            if (isNoLock)
                return _dbContext.Set<T>().AsNoTracking().ToListExtension().Result;
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> List(ISpecification<T> spec, bool isNoLock = true)
        {
            if (isNoLock)
                return ApplySpecification(spec).AsNoTracking().ToListExtension().Result;
            else
                return ApplySpecification(spec).AsNoTracking().ToList();
        }

        /// <summary>
        /// Use this method to get selected columns multiple rows return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <param name="propNames"></param>
        /// <returns></returns>
        public List<T> List(ISpecification<T> spec, string[] propNames, bool isNoLock = true)
        {
            var entity = ApplySpecification(spec).AsNoTracking();
            var parameter = Expression.Parameter(typeof(T), "e");
            var bindings = propNames
                .Select(propName =>
                {
                    var property = typeof(T).GetProperty(propName);
                    if (property == null)
                    {
                        throw new ArgumentException($"Property '{propName}' does not exist on type '{typeof(T).Name}'.");
                    }
                    return Expression.Bind(property, Expression.Property(parameter, property));
                })
                .ToArray();

            // Create a MemberInit expression with only the selected properties
            var selector = Expression.Lambda<Func<T, T>>(
                Expression.MemberInit(Expression.New(typeof(T)), bindings),
                parameter
            );
            return isNoLock ? entity.Select(selector).ToListExtension().Result : entity.Select(selector).ToList();
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter in async way.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<List<T>> ListAsync(ISpecification<T> spec, bool isNoLock = true)
        {
            if (isNoLock)
                return ApplySpecification(spec).AsNoTracking().ToListExtension();
            else
                return ApplySpecification(spec).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Use this method to get single column single value (scalar value) return referring to specification criteria and property name passed in parameter.
        /// The required column name is passed as string in propName parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetSingleValue(ISpecification<T> spec, string propName, bool isNoLock = true)
        {
            var entity = isNoLock ? ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsyncExtension().Result : ApplySpecification(spec).AsNoTracking().FirstOrDefault();
            if (entity == null) return null;
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
                return propertyInfo.GetValue(entity, null);
            return null;
        }

        /// <summary>
        /// Use this method to get single column with multiple values return referring to specification criteria and property name passed in parameter.
        /// The required column name is passed as string in propName parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetSingleValueList(ISpecification<T> spec, string propName, bool isNoLock = true)
        {
            var entity = isNoLock ? ApplySpecification(spec).AsNoTracking().ToListExtension().Result : ApplySpecification(spec).AsNoTracking().ToList();
            if (entity.Count > 0)
                return entity.AsQueryable().Select(propName);
            return null;
        }

        /// <summary>
        /// Use this method to get count of rows referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int Count(ISpecification<T> spec)
        {
            int results = ApplySpecification(spec).AsNoTracking().Count();
            return results;
        }

        /// <summary>
        /// Use this method to get count of rows referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Max(string columnName)
        {
            var result = _dbContext.Set<T>();
            return (from d in result
                    select d).Max(columnName);

        }

        /// <summary>
        /// This is special function and should be used to get database Datetime e.g. GetDate() from database server.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBDateTime()
        {
            FormattableString query = $"SELECT getdate()";
            var dQuery = _dbContext.Database.SqlQuery<DateTime>(query);
            return dQuery.AsEnumerable().First();
        }
        #endregion

        #region add methods
        /// <summary>
        /// Use this method to insert single entity data into database table referring to entity passed as parameter.
        /// It will also have primary key in entity itself which is used as parameter while calling the add method.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Use this method to insert single entity data into database table referring to entity passed as parameter.
        /// It will also have primary key in entity itself which is used as parameter while calling the add method.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="IsMultipleTxn"></param>
        public void Add(T entity, bool IsMultipleTxn)
        {
            _dbContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Use this method to insert multiple entities data into database with considering specification criteria, if passed then do not insert data into database table.
        /// If criteria is not passed then insert data into database table.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Add(ISpecification<T> spec, bool isNoLock = true)
        {
            var entity = isNoLock ? ApplySpecification(spec).FirstOrDefaultAsyncExtension().Result :
                ApplySpecification(spec).FirstOrDefault();
            if (entity == null)
            {
                _dbContext.Set<T>().Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Use this method to insert multiple entities data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Add(List<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            _dbContext.SaveChanges();
        }
        #endregion

        #region update methods
        /// <summary>
        /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
        /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
        /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="IncludeExclude"></param>
        public void Update(T entity, string[] properties, UpdateType IncludeExclude = UpdateType.Exclude)
        {
            _dbContext.Entry(entity).State = UpdateType.Include == IncludeExclude ? EntityState.Unchanged : EntityState.Modified;
            foreach (string property in properties)
            {
                _dbContext.Entry(entity).Property(property).IsModified = UpdateType.Include == IncludeExclude;
            }
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
        /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
        /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="IsMultipleTxn"></param>
        /// <param name="IncludeExclude"></param>
        public void Update(T entity, string[] properties, bool IsMultipleTxn, UpdateType IncludeExclude = UpdateType.Exclude)
        {
            _dbContext.Entry(entity).State = UpdateType.Include == IncludeExclude ? EntityState.Unchanged : EntityState.Modified;
            foreach (string property in properties)
            {
                _dbContext.Entry(entity).Property(property).IsModified = UpdateType.Include == IncludeExclude;
            }
        }

        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to specification criteria passed as parameter.
        /// User can also override the result based on criteria and it will update the result into database table
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Update(ISpecification<T> spec,  bool isNoLock = true)
        {
            List<T> results = isNoLock ? ApplySpecification(spec).AsNoTracking().ToListExtension().Result : ApplySpecification(spec).AsNoTracking().ToList();
            //Create Virtual Function to Override Result Based on Criteria [TBD: Murtuza]
            var newresult = SetUpdateEntityResult(results);
            _dbContext.UpdateRange(newresult);
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }
        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Update(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Use this method to update single entity all columns data into database table referring to entity passed as parameter.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to specification criteria passed as parameter.
        /// User can also override the result based on criteria and it will update the result into database table
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public int Update(ISpecification<T> spec, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            return ApplySpecification(spec).ExecuteUpdate(setPropertyCalls);
        }

        public List<T> SetUpdateEntityResult(List<T> results)
        {
            return results;
        }
        #endregion

        #region delete methods
        /// <summary>
        /// Use this method to delete single entity data from database table referring to single entity passed as parameter.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Use this method to delete multiple entities data referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Delete(ISpecification<T> spec)
        {
            var result = this.List(spec);
            _dbContext.Set<T>().RemoveRange(result);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Use this method to delete multiple entities data from database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Delete(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Use this method to temporary delete single entity data from database table referring to single entity passed as parameter.
        /// This method will do actual delete operation post calling save changes.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryToDelete(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }
        #endregion

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        /// <summary>
        /// Use this method to get page wise list of entities by specifying row start index, number of pages, sorting and specification criteria passed within parameters.
        /// And it will also return total items present in the list.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sorting"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual PaginedList<T> GetListByPage(int startIndex, int pageSize, string sorting, ISpecification<T> spec, bool isNoLock = true)
        {
            IQueryable<T> myQuery;
            myQuery = ApplySpecification(spec).AsNoTracking();
            if (!string.IsNullOrEmpty(sorting))
                return new PaginedList<T>
                {
                    Items = isNoLock ? myQuery.OrderBy(sorting).Skip(startIndex).Take(pageSize).ToListExtension().Result : myQuery.OrderBy(sorting).Skip(startIndex).Take(pageSize).ToList(),
                    TotalItems = startIndex == 0 ? myQuery.Count() : 0
                };
            else
                return new PaginedList<T>
                {
                    Items = isNoLock ? myQuery.Skip(startIndex).Take(pageSize).ToListExtension().Result :
                    myQuery.Skip(startIndex).Take(pageSize).ToList(),
                    TotalItems = startIndex == 0 ? myQuery.Count() : 0
                };
        }

        /// <summary>
        /// Use this method to get page wise list of selected columns of entities by specifying row start index, number of pages, sorting and specification criteria passed within parameters.
        /// And it will also return total items present in the list.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sorting"></param>
        /// <param name="spec"></param>
        /// <param name="propNames"></param>
        /// <returns></returns>
        public virtual async Task<PaginedList<T>> GetListByPageAsync(int startIndex, int pageSize, string sorting, ISpecification<T> spec, string[] propNames, bool isNoLock = true)
        {
            // Apply the specification and use AsNoTracking to improve performance
            IQueryable<T> myQuery = ApplySpecification(spec).AsNoTracking();

            // Build the selector expression for the specified properties
            if (propNames != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var bindings = propNames
                    .Select(propName =>
                    {
                        var property = typeof(T).GetProperty(propName);
                        if (property == null)
                        {
                            throw new ArgumentException($"Property '{propName}' does not exist on type '{typeof(T).Name}'.");
                        }
                        return Expression.Bind(property, Expression.Property(parameter, property));
                    })
                    .ToArray();

                var selector = Expression.Lambda<Func<T, T>>(
                    Expression.MemberInit(Expression.New(typeof(T)), bindings),
                    parameter
                );

                // Apply the selector once
                var query = myQuery.Select(selector);

                // Handle sorting logic if necessary
                if (!string.IsNullOrEmpty(sorting))
                {
                    query = ApplySorting(query, sorting);
                }

                // Calculate total items if on the first page, or use the pre-calculated count
                int totalItems = startIndex == 0 ? await myQuery.CountAsync() : 0;

                // Execute the query with pagination and optional no-lock behavior
                var items = isNoLock
                    ? await query.Skip(startIndex).Take(pageSize).ToListExtension()
                    : await query.Skip(startIndex).Take(pageSize).ToListAsync();

                return new PaginedList<T>
                {
                    Items = items,
                    TotalItems = totalItems
                };
            }

            return null;
        }

        // Helper method to apply sorting dynamically
        private static IQueryable<T> ApplySorting(IQueryable<T> query, string sorting)
        {
            // If sorting string is provided, assume it's a simple column name or a composite expression.
            // You can enhance this to handle dynamic expressions or multiple sort directions.
            var propertyInfo = typeof(T).GetProperty(sorting);
            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var property = Expression.Property(parameter, propertyInfo);
                var lambda = Expression.Lambda(property, parameter);
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);
                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
            }
            return query;
        }


        public static string GetKeyField(Type type)
        {
            PropertyInfo[] allProperties = type.GetProperties();

            PropertyInfo keyProperty = allProperties.SingleOrDefault(p => p.IsDefined(typeof(KeyAttribute)));

            return keyProperty?.Name;
        }

        #region Bulk Actions
        public void BulkInsert(List<T> entities)
        {
            Add(entities);
        }

        public void BulkUpdate(List<T> entities)
        {
            Update(entities);
        }

        public void BulkUpdate(List<T> entities, List<string> columns )
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<T>().Attach(entity);
                foreach (var column in columns)
                {
                    _dbContext.Entry(entity).Property(column).IsModified = true;
                }
            }
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }

        #endregion

        /// <summary>
        /// Use this method to check if data referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsItemExist(ISpecification<T> spec)
        {
            bool result = ApplySpecification(spec).AsNoTracking().Any();
            return result;
        }
    }
    public interface IGenericRepo<T, M> : IGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {
        /// <summary>
        /// This is virtual method to get list of entities based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> GetList<S>(S searchModel) where S : class
        {
            return new List<T>();
        }
        /// <summary>
        /// This is virtual method to get single entity based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public T GetUniqueRecordBy<S>(S searchModel) where S : class
        {
            return new T();
        }
    }
}
