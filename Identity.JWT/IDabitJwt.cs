using System;

namespace Identity.JWT
{
    public interface IDabitJwt
    {
        string GenerateToken(Guid userId);
    }
}