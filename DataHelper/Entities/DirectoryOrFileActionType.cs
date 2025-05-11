namespace DataHelper.Entities
{
    public enum DirectoryOrFileActionType : short
    {
        None = 0,
        DirectoryExists = 1,
        DirectoryCreate = 2,
        DirectoryDelete = 3,
        DirectoryRename = 4,
        DirectoryCopy = 5,
        DirectoryMove = 6,
        FileExists = 7,
        FileCreate = 8,
        FileDelete = 9,
        FileRename = 10,
        FileCopy = 11,
        FileMove = 12,
        FileStreamWrite = 13,
    }
}
