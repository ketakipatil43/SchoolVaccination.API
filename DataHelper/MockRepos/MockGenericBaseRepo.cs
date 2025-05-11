using DataHelper.EFData.Common.Interfaces;
using DataHelper.Entities;
using DataHelper.Entities.EnumFields;
using DataHelper.HelperClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace DataHelper.MockRepos
{

    public class MockGenericBaseRepo<T, M> : IGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {

        public MockGenericBaseRepo(M dbContext)
        {
            _dbContext = dbContext;
        }

        public M _dbContext { get; }

        #region search methods
        /// <summary>
        /// Use this method to get all columns single row return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public T GetUniqueRecordBySpec(ISpecification<T> spec, object message, bool asNoTracking = true)
        {
            T Entity;
            Entity = ApplySpecification(spec).FirstOrDefault();
            return Entity;
        }

        /// <summary>
        /// Use this method to get all columns all rows return without any specification criteria.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> ListAll(object message, bool isNoLock = true)
        {
            List<T> results = GetData<T>().ToList();
            return results;
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> List(ISpecification<T> spec, object message, bool isNoLock = true)
        {
            List<T> results = ApplySpecification(spec).ToList();
            return results;
        }

        public List<T> List(ISpecification<T> spec, object message, string[] propNames, bool isNoLock = true)
        {
            var entity = ApplySpecification(spec);
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
            return entity.Select(selector).ToList();
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter in async way.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<List<T>> ListAsync(ISpecification<T> spec, object message, bool isNoLock = true)
        {
            return ApplySpecification(spec).ToListAsync();
        }


        /// <summary>
        /// Use this method to get single column single value (scalar value) return referring to specification criteria and property name passed in parameter.
        /// The required column name is passed as string in propName parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetSingleValue(ISpecification<T> spec, string propName, object message, bool isNoLock = true)
        {
            var entity = ApplySpecification(spec).FirstOrDefault();
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
        public object GetSingleValueList(ISpecification<T> spec, string propName, object message, bool isNoLock = true)
        {
            var entity = ApplySpecification(spec).ToList();
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
        public int Count(ISpecification<T> spec, object message)
        {
            int results = ApplySpecification(spec).Count();
            return results;
        }

        /// <summary>
        /// Use this method to get count of rows referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Max(string columnName, object message)
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
        public void Add(T entity, object message)
        {
            /// <summary>
            /// Use this method to insert single entity data into database table referring to entity passed as parameter.
            /// It will also have primary key in entity itself which is used as parameter while calling the add method.
            /// </summary>
        }

        /// <summary>
        /// Use this method to insert multiple entities data into database with considering specification criteria, if passed then do not insert data into database table.
        /// If criteria is not passed then insert data into database table.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Add(ISpecification<T> spec, object message)
        {
            return true;
        }

        /// <summary>
        /// Use this method to insert single entity data into database table referring to entity passed as parameter.
        /// It will also have primary key in entity itself which is used as parameter while calling the add method.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Add(T entity, object message, bool IsMultipleTxn)
        {
            /// <summary>
            /// Use this method to insert single entity data into database table referring to entity passed as parameter.
            /// It will also have primary key in entity itself which is used as parameter while calling the add method.
            /// </summary>
        }

        /// <summary>
        /// Use this method to insert multiple entities data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Add(List<T> entities, object message)
        {
            /// <summary>
            /// Use this method to insert multiple entities data into database table referring to list of entities passed as parameter.
            /// </summary>

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
        public void Update(T entity, object message, string[] properties, UpdateType IncludeExclude = UpdateType.Exclude)
        {
            /// <summary>
            /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
            /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
            /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
            /// </summary>

        }

        /// <summary>
        /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
        /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
        /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="IncludeExclude"></param>
        public void Update(T entity, object message, string[] properties, bool IsMultipleTxn, UpdateType IncludeExclude = UpdateType.Exclude)
        {
            /// <summary>
            /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
            /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
            /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
            /// </summary>
        }

        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to specification criteria passed as parameter.
        /// User can also override the result based on criteria and it will update the result into database table
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Update(ISpecification<T> spec, object message)
        {
            /// <summary>
            /// Use this method to update multiple entities all columns data into database table referring to specification criteria passed as parameter.
            /// User can also override the result based on criteria and it will update the result into database table
            /// </summary>
        }

        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Update(List<T> entities, object message)
        {
            /// <summary>
            /// Use this method to update multiple entities all columns data into database table referring to list of entities passed as parameter.
            /// </summary>
        }

        /// <summary>
        /// Use this method to update single entity all columns data into database table referring to entity passed as parameter.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Update(T entity, object message)
        {
            /// <summary>
            /// Use this method to update single entity all columns data into database table referring to entity passed as parameter.
            /// </summary>
        }
        public int Update(ISpecification<T> spec, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, object message)
        {
            return 1;
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
        public void Delete(T entity, object message)
        {
            /// <summary>
            /// Use this method to delete single entity data from database table referring to single entity passed as parameter.
            /// </summary>

        }

        /// <summary>
        /// Use this method to delete multiple entities data referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Delete(ISpecification<T> spec, object message)
        {
            /// <summary>
            /// Use this method to delete multiple entities data referring to specification criteria passed as parameter.
            /// </summary>
        }

        /// <summary>
        /// Use this method to delete multiple entities data from database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Delete(List<T> entities, object message)
        {
            /// <summary>
            /// Use this method to delete multiple entities data from database table referring to list of entities passed as parameter.
            /// </summary>
        }

        /// <summary>
        /// Use this method to temporary delete single entity data from database table referring to single entity passed as parameter.
        /// This method will do actual delete operation post calling save changes.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryToDelete(T entity, object message)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }
        #endregion

        public bool IsItemExist(ISpecification<T> spec, object message)
        {
            bool result = ApplySpecification(spec).Any();
            return result;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(GetData<T>(), spec);
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
        /// Use this method to get page wise list of entities by specifying row start index, number of pages, sorting and specification criteria passed within parameters.
        /// And it will also return total items present in the list.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sorting"></param>
        /// <param name="spec"></param>
        /// <param name="propNames"></param>
        /// <param name="isNoLock"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public virtual async Task<PaginedList<T>> GetListByPageAsync(int startIndex, int pageSize, string sorting, ISpecification<T> spec, string[] propNames, bool isNoLock = true)
        {
            IQueryable<T> myQuery = ApplySpecification(spec).AsNoTracking();

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

                var query = myQuery.Select(selector);

                if (!string.IsNullOrEmpty(sorting))
                {
                    query = ApplySorting(query, sorting);
                }

              
                int totalItems = startIndex == 0 ? await Task.Run(() => myQuery.Count()) : 0;

                var items = await Task.Run(() => query.Skip(startIndex).Take(pageSize).ToList());


                return new PaginedList<T>
                {
                    Items = items,
                    TotalItems = totalItems
                };
            }

            return null;
        }
        private static IQueryable<T> ApplySorting(IQueryable<T> query, string sorting)
        {
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
        public void BulkInsert(List<T> entities, object message)
        {
            /// <summary>
            /// Use this method to insert multiple entities data into database table referring to list of entities passed as parameter.
            /// </summary>

        }

        public void BulkUpdate(List<T> entities, object message)
        {
            /// <summary>
            /// Use this method to update multiple entities data into database table referring to list of entities passed as parameter.
            /// </summary>
        }

       public static IQueryable<T> GetData<T>() where T : class, new()
{
    // Define the file path dynamically based on the entity type name
    var dataFileName = $"D:\\MoqData\\{typeof(T).Name}.json"; // Adjust path as necessary
    var items = new List<T>();

    try
    {
        // Check if the file exists
        if (File.Exists(dataFileName))
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Read the file and deserialize the content into the list of T
            using (var reader = new StreamReader(dataFileName))
            {
                var json = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    items = JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
                }
            }
        }
        else
        {
            // Log or handle the error when the file does not exist
            Console.WriteLine($"File not found: {dataFileName}");
        }
    }
    catch (Exception ex)
    {
        // Handle any exception (file read errors, JSON deserialization errors, etc.)
        Console.WriteLine($"Error reading or deserializing the file: {ex.Message}");
    }

    // Return the data as IQueryable for LINQ operations
    return items.AsQueryable();
}

        #endregion
    }

    public class MockGenericRepo<T, M> : MockGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {
        public MockGenericRepo(M dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// This is virtual method to get list of entities based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> GetList<S>(S searchModel, object message) where S : class
        {
            return new List<T>();
        }
        /// <summary>
        /// This is virtual method to get single entity based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public T GetUniqueRecordBySpec<S>(S searchModel, object message) where S : class
        {
            return new T();
        }
    }
}
