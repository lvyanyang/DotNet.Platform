// ===============================================================================
// Copyright (c) 2015 正得信集团股份有限公司
// ===============================================================================

using DotNet.Data;

namespace DotNet.Auth.Repository
{
    public class AuthRepository<T> : Repository<T> where T : class, new()
    {
        public AuthRepository() : base(new AuthDatabase()) { }
    }
}