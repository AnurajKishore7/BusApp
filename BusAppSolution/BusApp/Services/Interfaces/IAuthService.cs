using System.Threading.Tasks;
using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
        public interface IAuthService
        {
            Task<ClientRegister> RegisterClient(ClientRegisterDto request);
            Task<string> RegisterTransportOperator(TransportRegisterDto request);
            Task<LoginResponseDto> Login(LoginDto request);
            Task<bool> ApproveTransportOperator(string name);
        }
}
