using Volo.Abp.Account;

namespace Identi
{
    internal class tyUserDto : RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}