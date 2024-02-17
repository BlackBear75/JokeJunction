namespace JokeJunction.Domain.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        
        JokeNotFound = 10,
        
        OK = 200,
        AlreadyRated = 247,
        JokeRated = 267,


        NotJokeRated = 467,
        NotRemove = 469,
        InternalServerError = 500,
            BadRequest = 768
    }
}