using HousingComplex.Entities;

namespace HousingComplex.Security;

public interface IJwtUtils
{
    string GenerateToken(UserCredential userCredential);
}