using Microsoft.Data.SqlClient;
using DataHelper.Entities.EnumFields;
using DataHelper.HelperClasses;
using System.Data;
using System.Reflection;
using System.Text;

namespace DataHelper.SPData.Common.Repositories
{
    public class BaseRepository
    {
        protected StringBuilder _parametersSeq;
        protected string _spName;
        protected YesNoFlag _isTrimRequired = YesNoFlag.Yes;
        protected int? _outParamIndex;
        protected int? _structuredParamIndex;
        internal SPParamType _spParamType;
        public BaseRepository()
        {
            _parametersSeq = new StringBuilder();
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            var attr = BindingFlags.Public | BindingFlags.Instance;
            var dict = new Dictionary<string, object>();
            foreach (var property in data.GetType().GetProperties(attr))
            {
                if (property.CanRead)
                {
                    dict.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dict;
        }
        protected object[] SetParams(object inputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                if (_parametersSeq.Length > 0)
                    _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, object outputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            var outputparams = ToDictionary(outputparameters);
            int i = 0;
            var arr = new object[inputparams.Count + outputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                foreach (var one_kvp in outputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value,
                        Direction = ParameterDirection.Output
                    };
                    param.Size = param.DbType == DbType.String ? 2000 : param.Size;
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                if (_parametersSeq.Length > 0)
                    _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, bool IsOutParam)
        {
            var inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;

                    if (IsOutParam && _outParamIndex != null && i == _outParamIndex)
                    {
                        AddOutParam(arr, i);
                        i++;
                        i++;
                        _parametersSeq.Append(',');
                    }
                }
                if (!IsOutParam || _outParamIndex != null)
                {
                    if (_parametersSeq.Length > 0)
                        _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
                }
                else
                {
                    AddOutParam(arr, i);
                }
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, SPParamType paramtype, bool IsOutParam = false)
        {
            IDictionary<string, object> inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    if (i == _structuredParamIndex && paramtype == SPParamType.Structured)
                    {
                        param.SqlDbType = SqlDbType.Structured;
                        param.Direction = ParameterDirection.Input;
                    }
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;

                    if (IsOutParam && _outParamIndex != null && i == _outParamIndex)
                    {
                        AddOutParam(arr, i);
                        i++;
                        i++;
                        _parametersSeq.Append(',');
                    }
                }
                if (IsOutParam && _outParamIndex == null)
                {
                    AddOutParam(arr, i);
                }
                else
                {
                    if (_parametersSeq.Length > 0)
                        _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
                }
            }
            return arr;
        }
        private void AddOutParam(object[] arr, int startIndex)
        {

            var outparam = new SqlParameter
            {
                ParameterName = "PStatus",
                Direction = ParameterDirection.Output,
                DbType = DbType.Int32
            };

            arr[startIndex] = outparam;
            _parametersSeq.Append(" @PStatus OUT,");

            outparam = new SqlParameter
            {
                ParameterName = "PMsg",
                Direction = ParameterDirection.Output,
                DbType = DbType.String,
                Size = 60
            };
            arr[startIndex + 1] = outparam;
            _parametersSeq.Append(" @PMsg OUT");
        }

        #region Connection Related    
        public static string GetConnectionString()
        {
            return UserConfiguration.ConnectionBuilder;
        }
     
        protected List<T> ExtraSqlQueryWithDataReader<T>(object message, params object[] parameters)
        {
            return ExecuteSqlQuery<T>(_spName, CommandType.StoredProcedure, parameters);
        }

        protected List<T> ExtraSqlQueryWithDataReader<T>(string inlineQuery, object message)
        {
            return ExecuteSqlQuery<T>(inlineQuery, CommandType.Text);
        }
        private List<T> ExecuteSqlQuery<T>(string query, CommandType commandType, params object[] parameters)
        {
            List<T> result = new();
            try
            {
                using SqlConnection connection = new(GetConnectionString());
                using SqlCommand command = new(query, connection) { CommandType = commandType, CommandTimeout = 0 };

                if (parameters?.Length > 0)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var columnMappings = GetColumnMappings<T>(reader);

                    while (reader.Read())
                    {
                        T obj = MapDataReaderToEntity<T>(reader, columnMappings);
                        result.Add(obj);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex.InnerException ?? ex;
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
            return result;
        }

        private List<ResultEntityColumnMapping> GetColumnMappings<T>(SqlDataReader reader)
        {
            List<ResultEntityColumnMapping> mappings = new();
            var classProperties = typeof(T).GetProperties().ToList();

            int index = 0;
            foreach (DataRow colDetail in reader.GetSchemaTable().Rows)
            {
                string columnName = colDetail["ColumnName"].ToString();
                string mappedName = string.IsNullOrWhiteSpace(columnName) ? $"No_column_name{index}" : columnName.Trim()
                    .Replace(" ", "_").Replace(".", "_").Replace("(", "_").Replace(")", "_");

                string propertyName = classProperties
                    .Where(p => p.Name.Equals(mappedName, StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.Name)
                    .FirstOrDefault() ?? (columnName.Equals("default", StringComparison.OrdinalIgnoreCase) ? columnName + "1" : mappedName);

                mappings.Add(new ResultEntityColumnMapping
                {
                    SPColumnName = columnName,
                    SPColumnIndex = index,
                    ClassPropertyName = propertyName
                });

                index++;
            }
            return mappings;
        }

        private T MapDataReaderToEntity<T>(SqlDataReader reader, List<ResultEntityColumnMapping> mappings)
        {
            T obj = Activator.CreateInstance<T>();

            foreach (var col in mappings)
            {
                try
                {
                    PropertyInfo property = typeof(T).GetProperty(col.ClassPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.CanWrite)
                    {
                        object value = ConvertToPropType(property, reader[col.SPColumnIndex]);
                        property.SetValue(obj, value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }
            return obj;
        }


        protected object ConvertToPropType(PropertyInfo property, object value)
        {
            object cstVal = null;
            if (property != null)
            {
                Type propType = Nullable.GetUnderlyingType(property.PropertyType);
                bool isNullable = propType != null;
                if (!isNullable) { propType = property.PropertyType; }
                cstVal = value == null || Convert.IsDBNull(value) ? null : Convert.ChangeType(value, propType ?? property.PropertyType);
                if (cstVal != null && cstVal is string && _isTrimRequired == YesNoFlag.Yes)
                    cstVal = ((string)cstVal).Trim();
                else if (cstVal != null && cstVal is string && _isTrimRequired == YesNoFlag.No)
                    cstVal = (string)cstVal;
            }
            return cstVal;
        }
        #endregion
    }

    public class ResultEntityColumnMapping
    {
        public string SPColumnName { get; set; }
        public int SPColumnIndex { get; set; }
        public string ClassPropertyName { get; set; }
    }
}
