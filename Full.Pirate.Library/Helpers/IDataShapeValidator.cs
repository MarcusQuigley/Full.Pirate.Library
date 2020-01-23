namespace Full.Pirate.Library.Helpers
{
    public interface IDataShapeValidator
    {
        bool CheckFieldsExist<T>(string fields);
    }
}