namespace Full.Pirate.Library.Helpers
{
    public interface IDataShapeValidatorService
    {
        bool CheckFieldsExist<T>(string fields);
    }
}