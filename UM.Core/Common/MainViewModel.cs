namespace UM.Core.Common
{
    public class MainViewModel<T>
    {
        public T Data { get; set; }

        public MainViewModel()
        {
        }

        public MainViewModel(T entity)
        {
            Data = entity;
        }
    }
}
