namespace DbPeek.Models
{
    public enum ErrorCodes
    {
        GenericException = 1000,

        ValidationError = 2000,
        InvalidCharacters = ValidationError + 1,

        DatabaseError = 3000,
        ConnectionFailure = DatabaseError + 1,
        QueryError = DatabaseError + 2,

        UnknownError = 9000
    }
}
